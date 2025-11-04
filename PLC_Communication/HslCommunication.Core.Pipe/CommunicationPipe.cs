using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Pipe;

public abstract class CommunicationPipe : IDisposable
{
	private bool disposedValue;

	private int receiveTimeOut = 5000;

	private int sleepTime = 0;

	private bool useServerActivePush = false;

	private int connectErrorCount = 0;

	private ICommunicationLock communicationLock;

	protected AutoResetEvent autoResetEvent;

	protected byte[] bufferQA = null;

	protected bool isPersistentConn = false;

	public int ReceiveTimeOut
	{
		get
		{
			return receiveTimeOut;
		}
		set
		{
			receiveTimeOut = value;
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Get or set the time required to rest before officially receiving the data from the other party. When it is set to 0, no rest is required.")]
	public int SleepTime
	{
		get
		{
			return sleepTime;
		}
		set
		{
			sleepTime = value;
		}
	}

	public bool UseServerActivePush
	{
		get
		{
			return useServerActivePush;
		}
		set
		{
			if (value)
			{
				if (autoResetEvent == null)
				{
					autoResetEvent = new AutoResetEvent(initialState: false);
				}
				isPersistentConn = true;
			}
			useServerActivePush = value;
		}
	}

	public ICommunicationLock CommunicationLock
	{
		get
		{
			return communicationLock;
		}
		set
		{
			communicationLock = value;
		}
	}

	public bool IsPersistentConnection { get; set; } = true;

	public Func<CommunicationPipe, OperateResult<byte[]>, bool> DecideWhetherQAMessageFunction { get; set; }

	public CommunicationPipe()
	{
		communicationLock = new CommunicationLockSimple();
	}

	protected bool CheckMessageComplete(INetMessage netMessage, byte[] sendValue, ref MemoryStream ms)
	{
		if (netMessage == null)
		{
			return true;
		}
		if (netMessage is SpecifiedCharacterMessage specifiedCharacterMessage)
		{
			byte[] array = ms.ToArray();
			byte[] bytes = BitConverter.GetBytes(specifiedCharacterMessage.ProtocolHeadBytesLength);
			switch (bytes[3] & 0xF)
			{
			case 1:
				if (array.Length > specifiedCharacterMessage.EndLength && array[array.Length - 1 - specifiedCharacterMessage.EndLength] == bytes[1])
				{
					return true;
				}
				break;
			case 2:
				if (array.Length > specifiedCharacterMessage.EndLength + 1 && array[array.Length - 2 - specifiedCharacterMessage.EndLength] == bytes[1] && array[array.Length - 1 - specifiedCharacterMessage.EndLength] == bytes[0])
				{
					return true;
				}
				break;
			}
		}
		else if (netMessage.ProtocolHeadBytesLength > 0)
		{
			byte[] array2 = ms.ToArray();
			if (array2.Length >= netMessage.ProtocolHeadBytesLength)
			{
				int num = netMessage.PependedUselesByteLength(array2);
				if (num > 0)
				{
					array2 = array2.RemoveBegin(num);
					ms = new MemoryStream();
					ms.Write(array2);
					if (array2.Length < netMessage.ProtocolHeadBytesLength)
					{
						return false;
					}
				}
				netMessage.HeadBytes = array2.SelectBegin(netMessage.ProtocolHeadBytesLength);
				netMessage.SendBytes = sendValue;
				int contentLengthByHeadBytes = netMessage.GetContentLengthByHeadBytes();
				if (array2.Length >= netMessage.ProtocolHeadBytesLength + contentLengthByHeadBytes)
				{
					if (netMessage.ProtocolHeadBytesLength > netMessage.HeadBytes.Length)
					{
						ms = new MemoryStream();
						ms.Write(array2.RemoveBegin(netMessage.ProtocolHeadBytesLength - netMessage.HeadBytes.Length));
					}
					return true;
				}
			}
		}
		else if (netMessage.CheckReceiveDataComplete(sendValue, ms))
		{
			return true;
		}
		return false;
	}

	public int ResetConnectErrorCount()
	{
		return Interlocked.Exchange(ref connectErrorCount, 0);
	}

	protected int IncrConnectErrorCount()
	{
		int num = Interlocked.Increment(ref connectErrorCount);
		if (num > 1000000000)
		{
			Interlocked.Exchange(ref connectErrorCount, 1000000000);
		}
		return num;
	}

	public void RaisePipeError()
	{
		Interlocked.CompareExchange(ref connectErrorCount, 1, 0);
	}

	public virtual bool HasCacheData()
	{
		return false;
	}

	public virtual bool IsConnectError()
	{
		return connectErrorCount > 0;
	}

	public OperateResult Send(byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return Send(data, 0, data.Length);
	}

	public virtual OperateResult Send(byte[] data, int offset, int size)
	{
		return new OperateResult<int>(StringResources.Language.NotSupportedFunction);
	}

	public virtual OperateResult<byte[]> Receive(int length, int timeOut, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		OperateResult<byte[]> operateResult = NetSupport.CreateReceiveBuffer(length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<int> operateResult2 = Receive(operateResult.Content, 0, length, timeOut, reportProgress);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? operateResult.Content : operateResult.Content.SelectBegin(operateResult2.Content));
	}

	public virtual OperateResult<int> Receive(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		return new OperateResult<int>(StringResources.Language.NotSupportedFunction);
	}

	public virtual OperateResult StartReceiveBackground(INetMessage netMessage)
	{
		return OperateResult.CreateSuccessResult();
	}

	public virtual OperateResult<bool> OpenCommunication()
	{
		return new OperateResult<bool>(StringResources.Language.NotSupportedFunction);
	}

	public virtual OperateResult CloseCommunication()
	{
		return new OperateResult<int>(StringResources.Language.NotSupportedFunction);
	}

	protected void SetBufferQA(byte[] buffer)
	{
		bufferQA = buffer;
		autoResetEvent.Set();
	}

	public virtual OperateResult<byte[]> ReceiveMessage(INetMessage netMessage, byte[] sendValue, bool useActivePush = true, Action<long, long> reportProgress = null, Action<byte[]> logMessage = null)
	{
		if (useServerActivePush && useActivePush)
		{
			if (autoResetEvent.WaitOne(ReceiveTimeOut))
			{
				if (netMessage != null)
				{
					netMessage.HeadBytes = bufferQA;
				}
				logMessage?.Invoke(bufferQA);
				return OperateResult.CreateSuccessResult(bufferQA);
			}
			CloseCommunication();
			return new OperateResult<byte[]>(-IncrConnectErrorCount(), StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut);
		}
		if (netMessage == null || netMessage.ProtocolHeadBytesLength == -1)
		{
			if (netMessage != null && netMessage.SendBytes == null)
			{
				netMessage.SendBytes = sendValue;
			}
			DateTime now = DateTime.Now;
			MemoryStream memoryStream = new MemoryStream();
			do
			{
				OperateResult<byte[]> operateResult = ReceiveByMessage(ReceiveTimeOut, null, reportProgress);
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
				if (operateResult.Content != null && operateResult.Content.Length != 0)
				{
					memoryStream.Write(operateResult.Content);
					logMessage?.Invoke(operateResult.Content);
				}
				if (netMessage == null)
				{
					return OperateResult.CreateSuccessResult(memoryStream.ToArray());
				}
				if (netMessage.CheckReceiveDataComplete(sendValue, memoryStream))
				{
					return OperateResult.CreateSuccessResult(memoryStream.ToArray());
				}
			}
			while (ReceiveTimeOut < 0 || !((DateTime.Now - now).TotalMilliseconds > (double)ReceiveTimeOut));
			return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut + " Received: " + memoryStream.ToArray().ToHexString(' '));
		}
		OperateResult<byte[]> operateResult2 = ReceiveByMessage(ReceiveTimeOut, netMessage, reportProgress);
		if (operateResult2.IsSuccess)
		{
			logMessage?.Invoke(operateResult2.Content);
		}
		return operateResult2;
	}

	protected OperateResult<byte[]> ReadFromCoreServerHelper(INetMessage netMessage, byte[] sendValue, bool hasResponseData, int sleep, Action<byte[]> logMessage = null)
	{
		if (netMessage != null)
		{
			netMessage.SendBytes = sendValue;
		}
		OperateResult operateResult = Send(sendValue);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (!hasResponseData)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (sleep > 0)
		{
			HslHelper.ThreadSleep(sleep);
		}
		DateTime now = DateTime.Now;
		int num = 0;
		OperateResult<byte[]> operateResult2;
		while (true)
		{
			operateResult2 = ReceiveMessage(netMessage, sendValue, useActivePush: true, null, logMessage);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			if (netMessage == null)
			{
				break;
			}
			switch (netMessage.CheckMessageMatch(sendValue, operateResult2.Content))
			{
			case 0:
				return new OperateResult<byte[]>("INetMessage.CheckMessageMatch failed" + Environment.NewLine + StringResources.Language.Send + ": " + SoftBasic.ByteToHexString(sendValue, ' ') + Environment.NewLine + StringResources.Language.Receive + ": " + SoftBasic.ByteToHexString(operateResult2.Content, ' '));
			default:
				num++;
				if (ReceiveTimeOut < 0 || !((DateTime.Now - now).TotalMilliseconds > (double)ReceiveTimeOut))
				{
					continue;
				}
				return new OperateResult<byte[]>("Receive Message timeout: " + ReceiveTimeOut + " CheckMessageMatch times:" + num);
			case 1:
				break;
			}
			break;
		}
		if (netMessage != null && !netMessage.CheckHeadBytesLegal(null))
		{
			return new OperateResult<byte[]>(StringResources.Language.CommandHeadCodeCheckFailed + Environment.NewLine + StringResources.Language.Send + ": " + SoftBasic.ByteToHexString(sendValue, ' ') + Environment.NewLine + StringResources.Language.Receive + ": " + SoftBasic.ByteToHexString(operateResult2.Content, ' '));
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content);
	}

	public virtual OperateResult<byte[]> ReadFromCoreServer(INetMessage netMessage, byte[] sendValue, bool hasResponseData, Action<byte[]> logMessage = null)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServerHelper(netMessage, sendValue, hasResponseData, SleepTime, logMessage);
		if (!operateResult.IsSuccess)
		{
			if (operateResult.ErrorCode >= 0 || operateResult.ErrorCode != int.MinValue)
			{
			}
			return operateResult;
		}
		ResetConnectErrorCount();
		return operateResult;
	}

	private OperateResult<byte[]> ReceiveByMessage(int timeOut, INetMessage netMessage, Action<long, long> reportProgress = null)
	{
		if (netMessage == null)
		{
			return Receive(-1, timeOut);
		}
		if (netMessage.ProtocolHeadBytesLength < 0)
		{
			byte[] bytes = BitConverter.GetBytes(netMessage.ProtocolHeadBytesLength);
			int num = bytes[3] & 0xF;
			OperateResult<byte[]> operateResult = null;
			switch (num)
			{
			case 1:
				operateResult = ReceiveCommandLineFromPipe(bytes[1], timeOut);
				break;
			case 2:
				operateResult = ReceiveCommandLineFromPipe(bytes[1], bytes[0], timeOut);
				break;
			}
			if (operateResult == null)
			{
				return new OperateResult<byte[]>("Receive by specified code failed, length check failed");
			}
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			netMessage.HeadBytes = operateResult.Content;
			if (netMessage is SpecifiedCharacterMessage specifiedCharacterMessage)
			{
				if (specifiedCharacterMessage.EndLength == 0)
				{
					return operateResult;
				}
				OperateResult<byte[]> operateResult2 = Receive(specifiedCharacterMessage.EndLength, timeOut);
				if (!operateResult2.IsSuccess)
				{
					return operateResult2;
				}
				return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<byte>(operateResult.Content, operateResult2.Content));
			}
			return operateResult;
		}
		OperateResult<byte[]> operateResult3 = Receive(netMessage.ProtocolHeadBytesLength, timeOut);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		int num2 = netMessage.PependedUselesByteLength(operateResult3.Content);
		int num3 = 0;
		while (num2 >= netMessage.ProtocolHeadBytesLength)
		{
			operateResult3 = Receive(netMessage.ProtocolHeadBytesLength, timeOut);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			num2 = netMessage.PependedUselesByteLength(operateResult3.Content);
			num3++;
			if (num3 > 10)
			{
				break;
			}
		}
		if (num2 > 0)
		{
			OperateResult<byte[]> operateResult4 = Receive(num2, timeOut);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			operateResult3.Content = SoftBasic.SpliceArray<byte>(operateResult3.Content.RemoveBegin(num2), operateResult4.Content);
		}
		netMessage.HeadBytes = operateResult3.Content;
		int contentLengthByHeadBytes = netMessage.GetContentLengthByHeadBytes();
		if (contentLengthByHeadBytes <= 0)
		{
			return OperateResult.CreateSuccessResult(operateResult3.Content);
		}
		byte[] array = new byte[netMessage.HeadBytes.Length + contentLengthByHeadBytes];
		netMessage.HeadBytes.CopyTo(array, 0);
		OperateResult operateResult5 = Receive(array, netMessage.HeadBytes.Length, contentLengthByHeadBytes, timeOut, reportProgress);
		if (!operateResult5.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult5);
		}
		return OperateResult.CreateSuccessResult(array);
	}

	private OperateResult<byte[]> ReceiveCommandLineFromPipe(byte endCode, int timeout = 60000)
	{
		try
		{
			List<byte> list = new List<byte>(128);
			DateTime now = DateTime.Now;
			bool flag = false;
			while ((DateTime.Now - now).TotalMilliseconds < (double)timeout)
			{
				OperateResult<byte[]> operateResult = Receive(1, timeout);
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
				list.AddRange(operateResult.Content);
				if (operateResult.Content[0] == endCode)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + " " + timeout);
			}
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	private OperateResult<byte[]> ReceiveCommandLineFromPipe(byte endCode1, byte endCode2, int timeout = 60000)
	{
		try
		{
			List<byte> list = new List<byte>(128);
			DateTime now = DateTime.Now;
			bool flag = false;
			while ((DateTime.Now - now).TotalMilliseconds < (double)timeout)
			{
				OperateResult<byte[]> operateResult = Receive(1, timeout);
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
				list.AddRange(operateResult.Content);
				if (operateResult.Content[0] == endCode2 && list.Count > 1 && list[list.Count - 2] == endCode1)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + " " + timeout);
			}
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public async Task<OperateResult> SendAsync(byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return await SendAsync(data, 0, data.Length).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<OperateResult> SendAsync(byte[] data, int offset, int size)
	{
		return await Task.Run(() => Send(data, offset, size)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<OperateResult<byte[]>> ReceiveAsync(int length, int timeOut, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		OperateResult<byte[]> buffer = NetSupport.CreateReceiveBuffer(length);
		if (!buffer.IsSuccess)
		{
			return buffer;
		}
		OperateResult<int> receive = await ReceiveAsync(buffer.Content, 0, length, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(receive);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? buffer.Content : buffer.Content.SelectBegin(receive.Content));
	}

	public virtual async Task<OperateResult<int>> ReceiveAsync(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		return await Task.Run(() => Receive(buffer, offset, length, timeOut, reportProgress)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<OperateResult<bool>> OpenCommunicationAsync()
	{
		return await Task.Run(() => OpenCommunication()).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<OperateResult> CloseCommunicationAsync()
	{
		return await Task.Run(() => CloseCommunication()).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<OperateResult<byte[]>> ReceiveCommandLineFromPipeAsync(byte endCode, int timeout = 60000)
	{
		try
		{
			List<byte> bufferArray = new List<byte>(128);
			DateTime st = DateTime.Now;
			bool bOK = false;
			while ((DateTime.Now - st).TotalMilliseconds < (double)timeout)
			{
				OperateResult<byte[]> headResult = await ReceiveAsync(1, timeout).ConfigureAwait(continueOnCapturedContext: false);
				if (!headResult.IsSuccess)
				{
					return headResult;
				}
				bufferArray.AddRange(headResult.Content);
				if (headResult.Content[0] == endCode)
				{
					bOK = true;
					break;
				}
			}
			if (!bOK)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + " " + timeout);
			}
			return OperateResult.CreateSuccessResult(bufferArray.ToArray());
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			return new OperateResult<byte[]>(ex3.Message);
		}
	}

	private async Task<OperateResult<byte[]>> ReceiveCommandLineFromPipeAsync(byte endCode1, byte endCode2, int timeout = 60000)
	{
		try
		{
			List<byte> bufferArray = new List<byte>(128);
			DateTime st = DateTime.Now;
			bool bOK = false;
			while ((DateTime.Now - st).TotalMilliseconds < (double)timeout)
			{
				OperateResult<byte[]> headResult = await ReceiveAsync(1, timeout).ConfigureAwait(continueOnCapturedContext: false);
				if (!headResult.IsSuccess)
				{
					return headResult;
				}
				bufferArray.AddRange(headResult.Content);
				if (headResult.Content[0] == endCode2 && bufferArray.Count > 1 && bufferArray[bufferArray.Count - 2] == endCode1)
				{
					bOK = true;
					break;
				}
			}
			if (!bOK)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + " " + timeout);
			}
			return OperateResult.CreateSuccessResult(bufferArray.ToArray());
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			return new OperateResult<byte[]>(ex3.Message);
		}
	}

	private async Task<OperateResult<byte[]>> ReceiveByMessageAsync(int timeOut, INetMessage netMessage, Action<long, long> reportProgress = null)
	{
		if (netMessage == null)
		{
			return await ReceiveAsync(-1, timeOut).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (netMessage.ProtocolHeadBytesLength < 0)
		{
			byte[] headCode = BitConverter.GetBytes(netMessage.ProtocolHeadBytesLength);
			int codeLength = headCode[3] & 0xF;
			OperateResult<byte[]> receive = null;
			switch (codeLength)
			{
			case 1:
				receive = await ReceiveCommandLineFromPipeAsync(headCode[1], timeOut).ConfigureAwait(continueOnCapturedContext: false);
				break;
			case 2:
				receive = await ReceiveCommandLineFromPipeAsync(headCode[1], headCode[0], timeOut).ConfigureAwait(continueOnCapturedContext: false);
				break;
			}
			if (receive == null)
			{
				return new OperateResult<byte[]>("Receive by specified code failed, length check failed");
			}
			if (!receive.IsSuccess)
			{
				return receive;
			}
			netMessage.HeadBytes = receive.Content;
			if (netMessage is SpecifiedCharacterMessage message)
			{
				if (message.EndLength == 0)
				{
					return receive;
				}
				OperateResult<byte[]> endResult = await ReceiveAsync(message.EndLength, timeOut).ConfigureAwait(continueOnCapturedContext: false);
				if (!endResult.IsSuccess)
				{
					return endResult;
				}
				return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<byte>(receive.Content, endResult.Content));
			}
			return receive;
		}
		OperateResult<byte[]> headResult = await ReceiveAsync(netMessage.ProtocolHeadBytesLength, timeOut).ConfigureAwait(continueOnCapturedContext: false);
		if (!headResult.IsSuccess)
		{
			return headResult;
		}
		int start = netMessage.PependedUselesByteLength(headResult.Content);
		int cycleCount = 0;
		while (start >= netMessage.ProtocolHeadBytesLength)
		{
			headResult = await ReceiveAsync(netMessage.ProtocolHeadBytesLength, timeOut).ConfigureAwait(continueOnCapturedContext: false);
			if (!headResult.IsSuccess)
			{
				return headResult;
			}
			start = netMessage.PependedUselesByteLength(headResult.Content);
			cycleCount++;
			if (cycleCount > 10)
			{
				break;
			}
		}
		if (start > 0)
		{
			OperateResult<byte[]> head2Result = await ReceiveAsync(start, timeOut).ConfigureAwait(continueOnCapturedContext: false);
			if (!head2Result.IsSuccess)
			{
				return head2Result;
			}
			headResult.Content = SoftBasic.SpliceArray<byte>(headResult.Content.RemoveBegin(start), head2Result.Content);
		}
		netMessage.HeadBytes = headResult.Content;
		int contentLength = netMessage.GetContentLengthByHeadBytes();
		if (contentLength <= 0)
		{
			return OperateResult.CreateSuccessResult(headResult.Content);
		}
		byte[] result = new byte[netMessage.HeadBytes.Length + contentLength];
		netMessage.HeadBytes.CopyTo(result, 0);
		OperateResult contentResult = await ReceiveAsync(result, netMessage.HeadBytes.Length, contentLength, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (!contentResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(contentResult);
		}
		return OperateResult.CreateSuccessResult(result);
	}

	public virtual async Task<OperateResult<byte[]>> ReceiveMessageAsync(INetMessage netMessage, byte[] sendValue, bool useActivePush = true, Action<long, long> reportProgress = null, Action<byte[]> logMessage = null)
	{
		if (useServerActivePush && useActivePush)
		{
			if (autoResetEvent.WaitOne(ReceiveTimeOut))
			{
				if (netMessage != null)
				{
					netMessage.HeadBytes = bufferQA;
				}
				logMessage?.Invoke(bufferQA);
				return OperateResult.CreateSuccessResult(bufferQA);
			}
			CloseCommunication();
			return new OperateResult<byte[]>(-IncrConnectErrorCount(), StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut);
		}
		if (netMessage == null || netMessage.ProtocolHeadBytesLength == -1)
		{
			if (netMessage != null && netMessage.SendBytes == null)
			{
				netMessage.SendBytes = sendValue;
			}
			DateTime startTime = DateTime.Now;
			MemoryStream ms = new MemoryStream();
			do
			{
				OperateResult<byte[]> read2 = await ReceiveByMessageAsync(ReceiveTimeOut, null, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
				if (!read2.IsSuccess)
				{
					return read2;
				}
				if (read2.Content != null && read2.Content.Length != 0)
				{
					ms.Write(read2.Content);
					logMessage?.Invoke(read2.Content);
				}
				if (netMessage == null)
				{
					return OperateResult.CreateSuccessResult(ms.ToArray());
				}
				if (netMessage.CheckReceiveDataComplete(sendValue, ms))
				{
					return OperateResult.CreateSuccessResult(ms.ToArray());
				}
			}
			while (ReceiveTimeOut < 0 || !((DateTime.Now - startTime).TotalMilliseconds > (double)ReceiveTimeOut));
			return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + ReceiveTimeOut + " Received: " + ms.ToArray().ToHexString(' '));
		}
		OperateResult<byte[]> read3 = await ReceiveByMessageAsync(ReceiveTimeOut, netMessage, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (read3.IsSuccess)
		{
			logMessage?.Invoke(read3.Content);
		}
		return read3;
	}

	protected async Task<OperateResult<byte[]>> ReadFromCoreServerHelperAsync(INetMessage netMessage, byte[] sendValue, bool hasResponseData, int sleep, Action<byte[]> logMessage = null)
	{
		if (netMessage != null)
		{
			netMessage.SendBytes = sendValue;
		}
		OperateResult sendResult = await SendAsync(sendValue).ConfigureAwait(continueOnCapturedContext: false);
		if (!sendResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(sendResult);
		}
		if (ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (!hasResponseData)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (sleep > 0)
		{
			HslHelper.ThreadSleep(sleep);
		}
		DateTime start = DateTime.Now;
		int times = 0;
		OperateResult<byte[]> resultReceive;
		while (true)
		{
			resultReceive = await ReceiveMessageAsync(netMessage, sendValue, useActivePush: true, null, logMessage).ConfigureAwait(continueOnCapturedContext: false);
			if (!resultReceive.IsSuccess)
			{
				return resultReceive;
			}
			bool num;
			if (netMessage != null)
			{
				switch (netMessage.CheckMessageMatch(sendValue, resultReceive.Content))
				{
				case 0:
					return new OperateResult<byte[]>("INetMessage.CheckMessageMatch failed" + Environment.NewLine + StringResources.Language.Send + ": " + SoftBasic.ByteToHexString(sendValue, ' ') + Environment.NewLine + StringResources.Language.Receive + ": " + SoftBasic.ByteToHexString(resultReceive.Content, ' '));
				default:
					times++;
					num = ReceiveTimeOut >= 0 && (DateTime.Now - start).TotalMilliseconds > (double)ReceiveTimeOut;
					goto IL_0345;
				case 1:
					break;
				}
			}
			break;
			IL_0345:
			if (num)
			{
				return new OperateResult<byte[]>("Receive Message timeout: " + ReceiveTimeOut + " CheckMessageMatch times:" + times);
			}
		}
		if (netMessage != null && !netMessage.CheckHeadBytesLegal(null))
		{
			return new OperateResult<byte[]>(StringResources.Language.CommandHeadCodeCheckFailed + Environment.NewLine + StringResources.Language.Send + ": " + SoftBasic.ByteToHexString(sendValue, ' ') + Environment.NewLine + StringResources.Language.Receive + ": " + SoftBasic.ByteToHexString(resultReceive.Content, ' '));
		}
		return OperateResult.CreateSuccessResult(resultReceive.Content);
	}

	public virtual async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(INetMessage netMessage, byte[] sendValue, bool hasResponseData, Action<byte[]> logMessage = null)
	{
		OperateResult<byte[]> read = await ReadFromCoreServerHelperAsync(netMessage, sendValue, hasResponseData, SleepTime, logMessage).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			if (read.ErrorCode < 0 && read.ErrorCode != int.MinValue)
			{
			}
			return read;
		}
		ResetConnectErrorCount();
		return read;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			autoResetEvent?.Dispose();
			communicationLock?.Dispose();
		}
	}

	public void Dispose()
	{
		if (!disposedValue)
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
			disposedValue = true;
		}
	}
}
