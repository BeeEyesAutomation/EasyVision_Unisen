using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.Core.Net;

namespace HslCommunication.Core;

public static class NetSupport
{
	internal const int SocketBufferSize = 16384;

	public static int SocketErrorCode { get; } = -1;

	internal static int GetSplitLengthFromTotal(int length)
	{
		if (length < 1024)
		{
			return length;
		}
		if (length <= 8192)
		{
			return 2048;
		}
		if (length <= 32768)
		{
			return 8192;
		}
		if (length <= 262144)
		{
			return 32768;
		}
		if (length <= 1048576)
		{
			return 262144;
		}
		if (length <= 8388608)
		{
			return 1048576;
		}
		return 2097152;
	}

	internal static byte[] ReadBytesFromSocket(Socket socket, int receive, Action<long, long> reportProgress = null)
	{
		byte[] array = new byte[receive];
		ReceiveBytesFromSocket(socket, array, 0, receive, reportProgress);
		return array;
	}

	internal static void ReceiveBytesFromSocket(Socket socket, byte[] buffer, int offset, int length, Action<long, long> reportProgress = null)
	{
		int num = 0;
		while (num < length)
		{
			int size = Math.Min(length - num, 16384);
			int num2 = socket.Receive(buffer, num + offset, size, SocketFlags.None);
			num += num2;
			if (num2 == 0)
			{
				throw new RemoteCloseException();
			}
			reportProgress?.Invoke(num, length);
		}
	}

	internal static void ReceiveBytesFromSocket(Socket socket, Stream stream, int length, Action<long, long> reportProgress = null)
	{
		int num = 0;
		byte[] array = new byte[GetSplitLengthFromTotal(length)];
		while (num < length)
		{
			int num2 = socket.Receive(array, 0, array.Length, SocketFlags.None);
			stream.Write(array, 0, num2);
			num += num2;
			if (num2 == 0)
			{
				throw new RemoteCloseException();
			}
			reportProgress?.Invoke(num, length);
		}
	}

	internal static OperateResult<Socket> CreateSocketAndConnect(string ipAddress, int port, int timeOut, IPEndPoint local = null)
	{
		return CreateSocketAndConnect(new IPEndPoint(IPAddress.Parse(HslHelper.GetIpAddressFromInput(ipAddress)), port), timeOut, local);
	}

	internal static OperateResult<Socket> CreateSocketAndConnect(IPEndPoint endPoint, int timeOut, IPEndPoint local = null)
	{
		int num = 0;
		while (true)
		{
			num++;
			Socket socket = null;
			try
			{
				socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
			}
			catch (Exception ex)
			{
				return new OperateResult<Socket>("Socket Create Exception -> " + ex.Message);
			}
			HslTimeOut hslTimeOut = HslTimeOut.HandleTimeOutCheck(socket, timeOut);
			try
			{
				if (local != null)
				{
					socket.Bind(local);
				}
				socket.Connect(endPoint);
				hslTimeOut.IsSuccessful = true;
				return OperateResult.CreateSuccessResult(socket);
			}
			catch (Exception ex2)
			{
				socket?.Close();
				hslTimeOut.IsSuccessful = true;
				if (hslTimeOut.GetConsumeTime() < TimeSpan.FromMilliseconds(500.0) && num < 2)
				{
					HslHelper.ThreadSleep(100);
					continue;
				}
				if (hslTimeOut.IsTimeout)
				{
					return new OperateResult<Socket>(string.Format(StringResources.Language.ConnectTimeout, endPoint, timeOut) + " ms");
				}
				return new OperateResult<Socket>($"Socket Connect {endPoint} Exception -> " + ex2.Message);
			}
		}
	}

	public static OperateResult<byte[]> ReadFromCoreServer(IEnumerable<byte[]> send, Func<byte[], OperateResult<byte[]>> funcRead)
	{
		List<byte> list = new List<byte>();
		foreach (byte[] item in send)
		{
			OperateResult<byte[]> operateResult = funcRead(item);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			if (operateResult.Content != null)
			{
				list.AddRange(operateResult.Content);
			}
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	internal static async Task<OperateResult<Socket>> CreateSocketAndConnectAsync(IPEndPoint endPoint, int timeOut, IPEndPoint local = null)
	{
		int connectCount = 0;
		while (true)
		{
			connectCount++;
			Socket socket;
			try
			{
				socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
			}
			catch (Exception ex)
			{
				Exception ex3 = ex;
				Exception ex4 = ex3;
				return new OperateResult<Socket>("Socket Create Exception -> " + ex4.Message);
			}
			HslTimeOut connectTimeout = HslTimeOut.HandleTimeOutCheck(socket, timeOut);
			try
			{
				if (local != null)
				{
					socket.Bind(local);
				}
				await Task.Factory.FromAsync(socket.BeginConnect(endPoint, null, socket), socket.EndConnect).ConfigureAwait(continueOnCapturedContext: false);
				connectTimeout.IsSuccessful = true;
				return OperateResult.CreateSuccessResult(socket);
			}
			catch (Exception ex5)
			{
				connectTimeout.IsSuccessful = true;
				socket?.Close();
				if (!(connectTimeout.GetConsumeTime() < TimeSpan.FromMilliseconds(500.0)) || connectCount >= 2)
				{
					if (connectTimeout.IsTimeout)
					{
						return new OperateResult<Socket>(string.Format(StringResources.Language.ConnectTimeout, endPoint, timeOut) + " ms");
					}
					return new OperateResult<Socket>("Socket Exception -> " + ex5.Message);
				}
				await Task.Delay(100);
			}
		}
	}

	public static async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(IEnumerable<byte[]> send, Func<byte[], Task<OperateResult<byte[]>>> funcRead)
	{
		List<byte> array = new List<byte>();
		foreach (byte[] data in send)
		{
			OperateResult<byte[]> read = await funcRead(data).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				return read;
			}
			if (read.Content != null)
			{
				array.AddRange(read.Content);
			}
		}
		return OperateResult.CreateSuccessResult(array.ToArray());
	}

	public static void CloseSocket(Socket socket)
	{
		try
		{
			socket?.Close();
		}
		catch
		{
		}
	}

	public static OperateResult<byte[]> CreateReceiveBuffer(int length)
	{
		int num = ((length >= 0) ? length : 2048);
		try
		{
			return OperateResult.CreateSuccessResult(new byte[num]);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>($"Create byte[{num}] buffer failed: {ex.Message}");
		}
	}

	public static OperateResult<byte[]> SocketReceive(Socket socket, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<byte[]> operateResult = CreateReceiveBuffer(length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<int> operateResult2 = SocketReceive(socket, operateResult.Content, 0, length, timeOut, reportProgress);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? operateResult.Content : operateResult.Content.SelectBegin(operateResult2.Content));
	}

	public static OperateResult<int> SocketReceive(Socket socket, byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(0);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<int>(StringResources.Language.AuthorizationFailed);
		//}
		try
		{
			socket.ReceiveTimeout = timeOut;
			if (length > 0)
			{
				ReceiveBytesFromSocket(socket, buffer, offset, length, reportProgress);
				return OperateResult.CreateSuccessResult(length);
			}
			int num = socket.Receive(buffer, offset, buffer.Length - offset, SocketFlags.None);
			if (num == 0)
			{
				throw new RemoteCloseException();
			}
			return OperateResult.CreateSuccessResult(num);
		}
		catch (RemoteCloseException)
		{
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + StringResources.Language.RemoteClosedConnection);
		}
		catch (SocketException ex2)
		{
			if (ex2.SocketErrorCode == SocketError.TimedOut)
			{
				return new OperateResult<int>(SocketErrorCode, $"Socket Exception -> {ex2.Message} Timeout: {timeOut}")
				{
					Content = int.MaxValue
				};
			}
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + ex2.Message);
		}
		catch (Exception ex3)
		{
			return new OperateResult<int>(SocketErrorCode, "Exception -> " + ex3.Message);
		}
	}

	public static async Task<OperateResult<byte[]>> SocketReceiveAsync(Socket socket, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<byte[]> createBuffer = CreateReceiveBuffer(length);
		if (!createBuffer.IsSuccess)
		{
			return createBuffer;
		}
		OperateResult<int> receive = await SocketReceiveAsync(socket, createBuffer.Content, 0, length, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(receive);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? createBuffer.Content : createBuffer.Content.SelectBegin(receive.Content));
	}

	public static async Task<OperateResult<int>> SocketReceiveAsync(Socket socket, byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(length);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	new OperateResult<int>(StringResources.Language.AuthorizationFailed);
		//}
		HslTimeOut hslTimeOut = HslTimeOut.HandleTimeOutCheck(socket, timeOut);
		try
		{
			if (length > 0)
			{
				int alreadyCount = 0;
				do
				{
					int currentReceiveLength = ((length - alreadyCount > 16384) ? 16384 : (length - alreadyCount));
					int count = await Task.Factory.FromAsync(socket.BeginReceive(buffer, alreadyCount + offset, currentReceiveLength, SocketFlags.None, null, socket), (Func<IAsyncResult, int>)socket.EndReceive).ConfigureAwait(continueOnCapturedContext: false);
					alreadyCount += count;
					if (count > 0)
					{
						hslTimeOut.StartTime = DateTime.Now;
						reportProgress?.Invoke(alreadyCount, length);
						continue;
					}
					throw new RemoteCloseException();
				}
				while (alreadyCount < length);
				hslTimeOut.IsSuccessful = true;
				return OperateResult.CreateSuccessResult(length);
			}
			int count2 = await Task.Factory.FromAsync(socket.BeginReceive(buffer, offset, buffer.Length - offset, SocketFlags.None, null, socket), (Func<IAsyncResult, int>)socket.EndReceive).ConfigureAwait(continueOnCapturedContext: false);
			if (count2 == 0)
			{
				throw new RemoteCloseException();
			}
			hslTimeOut.IsSuccessful = true;
			return OperateResult.CreateSuccessResult(count2);
		}
		catch (RemoteCloseException)
		{
			socket?.Close();
			hslTimeOut.IsSuccessful = true;
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + StringResources.Language.RemoteClosedConnection);
		}
		catch (Exception ex2)
		{
			socket?.Close();
			hslTimeOut.IsSuccessful = true;
			if (hslTimeOut.IsTimeout)
			{
				return new OperateResult<int>(SocketErrorCode, $"Socket Exception -> {StringResources.Language.ReceiveDataTimeout}{hslTimeOut.DelayTime}");
			}
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + ex2.Message);
		}
	}

	public static async Task<OperateResult<int>> SocketReceiveAsync2(Socket socket, byte[] buffer, int offset, int length, int timeOut, Action<long, long> reportProgress, Func<IAsyncResult, int> endMethod)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(length);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	new OperateResult<int>(StringResources.Language.AuthorizationFailed);
		//}
		HslTimeOut hslTimeOut = HslTimeOut.HandleTimeOutCheck(socket, timeOut);
		try
		{
			if (length > 0)
			{
				int alreadyCount = 0;
				do
				{
					int currentReceiveLength = ((length - alreadyCount > 16384) ? 16384 : (length - alreadyCount));
					int count = await Task.Factory.FromAsync(socket.BeginReceive(buffer, alreadyCount + offset, currentReceiveLength, SocketFlags.None, null, socket), endMethod).ConfigureAwait(continueOnCapturedContext: false);
					alreadyCount += count;
					if (count > 0)
					{
						hslTimeOut.StartTime = DateTime.Now;
						reportProgress?.Invoke(alreadyCount, length);
						continue;
					}
					throw new RemoteCloseException();
				}
				while (alreadyCount < length);
				hslTimeOut.IsSuccessful = true;
				return OperateResult.CreateSuccessResult(length);
			}
			int count2 = await Task.Factory.FromAsync(socket.BeginReceive(buffer, offset, buffer.Length - offset, SocketFlags.None, null, socket), endMethod).ConfigureAwait(continueOnCapturedContext: false);
			if (count2 == 0)
			{
				throw new RemoteCloseException();
			}
			hslTimeOut.IsSuccessful = true;
			return OperateResult.CreateSuccessResult(count2);
		}
		catch (RemoteCloseException)
		{
			socket?.Close();
			hslTimeOut.IsSuccessful = true;
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + StringResources.Language.RemoteClosedConnection);
		}
		catch (Exception ex2)
		{
			socket?.Close();
			hslTimeOut.IsSuccessful = true;
			if (hslTimeOut.IsTimeout)
			{
				return new OperateResult<int>(SocketErrorCode, $"Socket Exception -> {StringResources.Language.ReceiveDataTimeout}{hslTimeOut.DelayTime}");
			}
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + ex2.Message);
		}
	}

	public static OperateResult<byte[]> SocketReceive(SslStream ssl, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<byte[]> operateResult = CreateReceiveBuffer(length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<int> operateResult2 = SocketReceive(ssl, operateResult.Content, 0, length, timeOut, reportProgress);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? operateResult.Content : operateResult.Content.SelectBegin(operateResult2.Content));
	}

	public static OperateResult<int> SocketReceive(SslStream ssl, byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(0);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<int>(StringResources.Language.AuthorizationFailed);
		//}
		try
		{
			ssl.ReadTimeout = timeOut;
			if (length > 0)
			{
				int num = 0;
				while (num < length)
				{
					int count = Math.Min(length - num, 16384);
					int num2 = ssl.Read(buffer, num + offset, count);
					num += num2;
					if (num2 == 0)
					{
						throw new RemoteCloseException();
					}
					reportProgress?.Invoke(num, length);
				}
				return OperateResult.CreateSuccessResult(length);
			}
			int num3 = ssl.Read(buffer, offset, buffer.Length - offset);
			if (num3 == 0)
			{
				throw new RemoteCloseException();
			}
			return OperateResult.CreateSuccessResult(num3);
		}
		catch (RemoteCloseException)
		{
			ssl?.Close();
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + StringResources.Language.RemoteClosedConnection);
		}
		catch (Exception ex2)
		{
			ssl?.Close();
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + ex2.Message);
		}
	}

	public static async Task<OperateResult<byte[]>> SocketReceiveAsync(SslStream ssl, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<byte[]> createBuffer = CreateReceiveBuffer(length);
		if (!createBuffer.IsSuccess)
		{
			return createBuffer;
		}
		OperateResult<int> receive = await SocketReceiveAsync(ssl, createBuffer.Content, 0, length, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(receive);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? createBuffer.Content : createBuffer.Content.SelectBegin(receive.Content));
	}

	public static async Task<OperateResult<int>> SocketReceiveAsync(SslStream ssl, byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(length);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	new OperateResult<int>(StringResources.Language.AuthorizationFailed);
		//}
		try
		{
			if (length > 0)
			{
				int alreadyCount = 0;
				do
				{
					int currentReceiveLength = ((length - alreadyCount > 16384) ? 16384 : (length - alreadyCount));
					int count = await ssl.ReadAsync(buffer, alreadyCount + offset, currentReceiveLength).ConfigureAwait(continueOnCapturedContext: false);
					alreadyCount += count;
					if (count == 0)
					{
						throw new RemoteCloseException();
					}
					reportProgress?.Invoke(alreadyCount, length);
				}
				while (alreadyCount < length);
				return OperateResult.CreateSuccessResult(length);
			}
			int count2 = await ssl.ReadAsync(buffer, offset, buffer.Length - offset).ConfigureAwait(continueOnCapturedContext: false);
			if (count2 == 0)
			{
				throw new RemoteCloseException();
			}
			return OperateResult.CreateSuccessResult(count2);
		}
		catch (RemoteCloseException)
		{
			ssl?.Close();
			return new OperateResult<int>(SocketErrorCode, StringResources.Language.RemoteClosedConnection);
		}
		catch (Exception ex2)
		{
			ssl?.Close();
			return new OperateResult<int>(SocketErrorCode, "Socket Exception -> " + ex2.Message);
		}
	}

	public static OperateResult SocketSend(Socket socket, byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return SocketSend(socket, data, 0, data.Length);
	}

	public static OperateResult SocketSend(Socket socket, byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		if (socket == null)
		{
			return new OperateResult<byte[]>(SocketErrorCode, "Socket is null");
		}
		try
		{
			int num = 0;
			do
			{
				int num2 = socket.Send(data, offset, size - num, SocketFlags.None);
				num += num2;
				offset += num2;
			}
			while (num < size && num < data.Length);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<byte[]>(SocketErrorCode, ex.Message);
		}
	}

	public static async Task<OperateResult> SocketSendAsync(Socket socket, byte[] data)
	{
		if (data == null)
		{
			return await Task.FromResult(OperateResult.CreateSuccessResult()).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await SocketSendAsync(socket, data, 0, data.Length).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static async Task<OperateResult> SocketSendAsync(Socket socket, byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult(StringResources.Language.AuthorizationFailed);
		//}
		if (socket == null)
		{
			return new OperateResult<byte[]>(SocketErrorCode, "Socket is null");
		}
		int sendCount = 0;
		try
		{
			do
			{
				int count = await Task.Factory.FromAsync(socket.BeginSend(data, offset, size - sendCount, SocketFlags.None, null, socket), (Func<IAsyncResult, int>)socket.EndSend).ConfigureAwait(continueOnCapturedContext: false);
				sendCount += count;
				offset += count;
			}
			while (sendCount < size && sendCount < data.Length);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<byte[]>(SocketErrorCode, ex.Message);
		}
	}

	public static OperateResult SocketSend(SslStream ssl, byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return SocketSend(ssl, data, 0, data.Length);
	}

	public static OperateResult SocketSend(SslStream ssl, byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		if (ssl == null)
		{
			return new OperateResult(SocketErrorCode, "SslStream is null");
		}
		try
		{
			ssl.Write(data, offset, size);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			ssl?.Close();
			return new OperateResult<byte[]>(SocketErrorCode, ex.Message);
		}
	}

	public static async Task<OperateResult> SocketSendAsync(SslStream ssl, byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return await SocketSendAsync(ssl, data, 0, data.Length).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static async Task<OperateResult> SocketSendAsync(SslStream ssl, byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		if (ssl == null)
		{
			return new OperateResult(SocketErrorCode, "SslStream is null");
		}
		try
		{
			await ssl.WriteAsync(data, offset, size).ConfigureAwait(continueOnCapturedContext: false);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			ssl?.Close();
			return new OperateResult<byte[]>(SocketErrorCode, ex.Message);
		}
	}

	public static OperateResult<int> ReadStream(Stream stream, byte[] buffer)
	{
		ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
		FileStateObject fileStateObject = new FileStateObject
		{
			WaitDone = manualResetEvent,
			Stream = stream,
			DataLength = buffer.Length,
			Buffer = buffer
		};
		try
		{
			stream.BeginRead(buffer, 0, fileStateObject.DataLength, ReadStreamCallBack, fileStateObject);
		}
		catch (Exception ex)
		{
			fileStateObject = null;
			manualResetEvent.Close();
			return new OperateResult<int>("stream.BeginRead Exception -> " + ex.Message);
		}
		manualResetEvent.WaitOne();
		manualResetEvent.Close();
		return fileStateObject.IsError ? new OperateResult<int>(fileStateObject.ErrerMsg) : OperateResult.CreateSuccessResult(fileStateObject.AlreadyDealLength);
	}

	private static void ReadStreamCallBack(IAsyncResult ar)
	{
		if (ar.AsyncState is FileStateObject fileStateObject)
		{
			try
			{
				fileStateObject.AlreadyDealLength += fileStateObject.Stream.EndRead(ar);
				fileStateObject.WaitDone.Set();
			}
			catch (Exception ex)
			{
				fileStateObject.IsError = true;
				fileStateObject.ErrerMsg = ex.Message;
				fileStateObject.WaitDone.Set();
			}
		}
	}

	public static OperateResult WriteStream(Stream stream, byte[] buffer)
	{
		ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
		FileStateObject fileStateObject = new FileStateObject
		{
			WaitDone = manualResetEvent,
			Stream = stream
		};
		try
		{
			stream.BeginWrite(buffer, 0, buffer.Length, WriteStreamCallBack, fileStateObject);
		}
		catch (Exception ex)
		{
			fileStateObject = null;
			manualResetEvent.Close();
			return new OperateResult("stream.BeginWrite Exception -> " + ex.Message);
		}
		manualResetEvent.WaitOne();
		manualResetEvent.Close();
		if (fileStateObject.IsError)
		{
			return new OperateResult
			{
				Message = fileStateObject.ErrerMsg
			};
		}
		return OperateResult.CreateSuccessResult();
	}

	private static void WriteStreamCallBack(IAsyncResult ar)
	{
		if (!(ar.AsyncState is FileStateObject fileStateObject))
		{
			return;
		}
		try
		{
			fileStateObject.Stream.EndWrite(ar);
		}
		catch (Exception ex)
		{
			fileStateObject.IsError = true;
			fileStateObject.ErrerMsg = ex.Message;
		}
		finally
		{
			fileStateObject.WaitDone.Set();
		}
	}

	public static async Task<OperateResult<int>> ReadStreamAsync(Stream stream, byte[] buffer)
	{
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<int>(StringResources.Language.AuthorizationFailed);
		//}
		try
		{
			return OperateResult.CreateSuccessResult(await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(continueOnCapturedContext: false));
		}
		catch (Exception ex)
		{
			stream?.Close();
			return new OperateResult<int>(ex.Message);
		}
	}

	public static async Task<OperateResult> WriteStreamAsync(Stream stream, byte[] buffer)
	{
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult(StringResources.Language.AuthorizationFailed);
		//}
		int alreadyCount = 0;
		try
		{
			await stream.WriteAsync(buffer, alreadyCount, buffer.Length - alreadyCount).ConfigureAwait(continueOnCapturedContext: false);
			return OperateResult.CreateSuccessResult(alreadyCount);
		}
		catch (Exception ex)
		{
			stream?.Close();
			return new OperateResult<int>(ex.Message);
		}
	}
}
