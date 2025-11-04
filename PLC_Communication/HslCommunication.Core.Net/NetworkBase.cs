using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Security;
using HslCommunication.Enthernet.Redis;
using HslCommunication.LogNet;
using HslCommunication.MQTT;

namespace HslCommunication.Core.Net;

public abstract class NetworkBase
{
	protected int fileCacheSize = 102400;

	private int connectErrorCount = 0;

	public ILogNet LogNet { get; set; }

	public Guid Token { get; set; }

	public NetworkBase()
	{
		Token = Guid.Empty;
		Authorization.oasjodaiwfsodopsdjpasjpf();
	}

	protected OperateResult<int> Receive(Socket socket, byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
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
				NetSupport.ReceiveBytesFromSocket(socket, buffer, offset, length, reportProgress);
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
			socket?.Close();
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<int>(-connectErrorCount, "Socket Exception -> " + StringResources.Language.RemoteClosedConnection);
		}
		catch (Exception ex2)
		{
			socket?.Close();
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<int>(-connectErrorCount, "Socket Exception -> " + ex2.Message);
		}
	}

	protected OperateResult<byte[]> Receive(Socket socket, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		int num = ((length > 0) ? length : 2048);
		byte[] array;
		try
		{
			array = new byte[num];
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<byte[]>($"Create byte[{num}] buffer failed: " + ex.Message);
		}
		OperateResult<int> operateResult = Receive(socket, array, 0, length, timeOut, reportProgress);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? array : array.SelectBegin(operateResult.Content));
	}

	protected OperateResult<int> Receive(SslStream ssl, byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
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
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<int>(-1, "Socket Exception -> " + StringResources.Language.RemoteClosedConnection);
		}
		catch (Exception ex2)
		{
			ssl?.Close();
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<int>(-1, "Socket Exception -> " + ex2.Message);
		}
	}

	protected OperateResult<byte[]> Receive(SslStream ssl, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		int num = ((length > 0) ? length : 2048);
		byte[] array;
		try
		{
			array = new byte[num];
		}
		catch (Exception ex)
		{
			ssl?.Close();
			return new OperateResult<byte[]>($"Create byte[{num}] buffer failed: " + ex.Message);
		}
		OperateResult<int> operateResult = Receive(ssl, array, 0, length, timeOut, reportProgress);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? array : array.SelectBegin(operateResult.Content));
	}

	protected OperateResult<byte[]> ReceiveCommandLineFromSocket(Socket socket, byte endCode, int timeout = 60000)
	{
		List<byte> list = new List<byte>(128);
		try
		{
			DateTime now = DateTime.Now;
			bool flag = false;
			while ((DateTime.Now - now).TotalMilliseconds < (double)timeout)
			{
				if (socket.Poll(timeout, SelectMode.SelectRead))
				{
					OperateResult<byte[]> operateResult = Receive(socket, 1, timeout);
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
			}
			if (!flag)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout);
			}
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	protected OperateResult<byte[]> ReceiveCommandLineFromSocket(Socket socket, byte endCode1, byte endCode2, int timeout = 60000)
	{
		List<byte> list = new List<byte>(128);
		try
		{
			DateTime now = DateTime.Now;
			bool flag = false;
			while ((DateTime.Now - now).TotalMilliseconds < (double)timeout)
			{
				if (socket.Poll(timeout, SelectMode.SelectRead))
				{
					OperateResult<byte[]> operateResult = Receive(socket, 1, timeout);
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
			}
			if (!flag)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout);
			}
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	protected virtual OperateResult<byte[]> ReceiveByMessage(Socket socket, int timeOut, INetMessage netMessage, Action<long, long> reportProgress = null)
	{
		if (netMessage == null)
		{
			return Receive(socket, -1, timeOut);
		}
		if (netMessage.ProtocolHeadBytesLength < 0)
		{
			byte[] bytes = BitConverter.GetBytes(netMessage.ProtocolHeadBytesLength);
			int num = bytes[3] & 0xF;
			OperateResult<byte[]> operateResult = null;
			switch (num)
			{
			case 1:
				operateResult = ReceiveCommandLineFromSocket(socket, bytes[1], timeOut);
				break;
			case 2:
				operateResult = ReceiveCommandLineFromSocket(socket, bytes[1], bytes[0], timeOut);
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
				OperateResult<byte[]> operateResult2 = Receive(socket, specifiedCharacterMessage.EndLength, timeOut);
				if (!operateResult2.IsSuccess)
				{
					return operateResult2;
				}
				return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<byte>(operateResult.Content, operateResult2.Content));
			}
			return operateResult;
		}
		OperateResult<byte[]> operateResult3 = Receive(socket, netMessage.ProtocolHeadBytesLength, timeOut);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		int num2 = netMessage.PependedUselesByteLength(operateResult3.Content);
		int num3 = 0;
		while (num2 >= netMessage.ProtocolHeadBytesLength)
		{
			operateResult3 = Receive(socket, netMessage.ProtocolHeadBytesLength, timeOut);
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
			OperateResult<byte[]> operateResult4 = Receive(socket, num2, timeOut);
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
		byte[] array = new byte[netMessage.ProtocolHeadBytesLength + contentLengthByHeadBytes];
		operateResult3.Content.CopyTo(array, 0);
		OperateResult operateResult5 = Receive(socket, array, netMessage.ProtocolHeadBytesLength, contentLengthByHeadBytes, timeOut, reportProgress);
		if (!operateResult5.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult5);
		}
		return OperateResult.CreateSuccessResult(array);
	}

	protected OperateResult Send(Socket socket, byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return Send(socket, data, 0, data.Length);
	}

	protected OperateResult Send(Socket socket, byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		try
		{
			int num = 0;
			do
			{
				int num2 = socket.Send(data, offset, size - num, SocketFlags.None);
				num += num2;
				offset += num2;
			}
			while (num < size);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			socket?.Close();
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<byte[]>(-connectErrorCount, ex.Message);
		}
	}

	protected OperateResult Send(SslStream ssl, byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		try
		{
			ssl.Write(data, offset, size);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			ssl?.Close();
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<byte[]>(-connectErrorCount, ex.Message);
		}
	}

	protected OperateResult Send(SslStream ssl, byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return Send(ssl, data, 0, data.Length);
	}

	protected OperateResult<Socket> CreateSocketAndConnect(string ipAddress, int port)
	{
		return CreateSocketAndConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), 10000);
	}

	protected OperateResult<Socket> CreateSocketAndConnect(string ipAddress, int port, int timeOut)
	{
		return CreateSocketAndConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), timeOut);
	}

	protected OperateResult<Socket> CreateSocketAndConnect(IPEndPoint endPoint, int timeOut, IPEndPoint local = null)
	{
		OperateResult<Socket> operateResult = NetSupport.CreateSocketAndConnect(endPoint, timeOut, local);
		if (operateResult.IsSuccess)
		{
			connectErrorCount = 0;
			return operateResult;
		}
		if (connectErrorCount < 1000000000)
		{
			connectErrorCount++;
		}
		return new OperateResult<Socket>(-connectErrorCount, operateResult.Message);
	}

	protected bool CheckRemoteToken(byte[] headBytes)
	{
		return SoftBasic.IsByteTokenEquel(headBytes, Token);
	}

	protected OperateResult SendBaseAndCheckReceive(Socket socket, int headCode, int customer, byte[] send)
	{
		send = HslProtocol.CommandBytes(headCode, customer, Token, send);
		OperateResult operateResult = Send(socket, send);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<long> operateResult2 = ReceiveLong(socket);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if (operateResult2.Content != send.Length)
		{
			socket?.Close();
			return new OperateResult(StringResources.Language.CommandLengthCheckFailed);
		}
		return operateResult2;
	}

	protected OperateResult SendBytesAndCheckReceive(Socket socket, int customer, byte[] send)
	{
		return SendBaseAndCheckReceive(socket, 1002, customer, send);
	}

	protected OperateResult SendStringAndCheckReceive(Socket socket, int customer, string send)
	{
		byte[] send2 = (string.IsNullOrEmpty(send) ? null : Encoding.Unicode.GetBytes(send));
		return SendBaseAndCheckReceive(socket, 1001, customer, send2);
	}

	protected OperateResult SendStringAndCheckReceive(Socket socket, int customer, string[] sends)
	{
		return SendBaseAndCheckReceive(socket, 1005, customer, HslProtocol.PackStringArrayToByte(sends));
	}

	protected OperateResult SendAccountAndCheckReceive(Socket socket, int customer, string name, string pwd)
	{
		return SendBaseAndCheckReceive(socket, 5, customer, HslProtocol.PackStringArrayToByte(new string[2] { name, pwd }));
	}

	protected OperateResult<byte[], byte[]> ReceiveAndCheckBytes(Socket socket, int timeOut)
	{
		OperateResult<byte[]> operateResult = Receive(socket, 32, timeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<byte[], byte[]>();
		}
		if (!CheckRemoteToken(operateResult.Content))
		{
			socket?.Close();
			return new OperateResult<byte[], byte[]>(StringResources.Language.TokenCheckFailed);
		}
		int num = BitConverter.ToInt32(operateResult.Content, 28);
		OperateResult<byte[]> operateResult2 = Receive(socket, num, timeOut);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2.ConvertFailed<byte[], byte[]>();
		}
		OperateResult operateResult3 = SendLong(socket, 32 + num);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3.ConvertFailed<byte[], byte[]>();
		}
		byte[] content = operateResult.Content;
		byte[] content2 = operateResult2.Content;
		content2 = HslProtocol.CommandAnalysis(content, content2);
		return OperateResult.CreateSuccessResult(content, content2);
	}

	protected OperateResult<int, string> ReceiveStringContentFromSocket(Socket socket, int timeOut = 30000)
	{
		OperateResult<byte[], byte[]> operateResult = ReceiveAndCheckBytes(socket, timeOut);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, string>(operateResult);
		}
		if (BitConverter.ToInt32(operateResult.Content1, 0) != 1001)
		{
			socket?.Close();
			return new OperateResult<int, string>(StringResources.Language.CommandHeadCodeCheckFailed);
		}
		if (operateResult.Content2 == null)
		{
			operateResult.Content2 = new byte[0];
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToInt32(operateResult.Content1, 4), Encoding.Unicode.GetString(operateResult.Content2));
	}

	protected OperateResult<int, string[]> ReceiveStringArrayContentFromSocket(Socket socket, int timeOut = 30000)
	{
		OperateResult<byte[], byte[]> operateResult = ReceiveAndCheckBytes(socket, timeOut);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, string[]>(operateResult);
		}
		if (BitConverter.ToInt32(operateResult.Content1, 0) != 1005)
		{
			socket?.Close();
			return new OperateResult<int, string[]>(StringResources.Language.CommandHeadCodeCheckFailed);
		}
		if (operateResult.Content2 == null)
		{
			operateResult.Content2 = new byte[4];
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToInt32(operateResult.Content1, 4), HslProtocol.UnPackStringArrayFromByte(operateResult.Content2));
	}

	protected OperateResult<int, byte[]> ReceiveBytesContentFromSocket(Socket socket, int timeout = 30000)
	{
		OperateResult<byte[], byte[]> operateResult = ReceiveAndCheckBytes(socket, timeout);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, byte[]>(operateResult);
		}
		if (BitConverter.ToInt32(operateResult.Content1, 0) != 1002)
		{
			socket?.Close();
			return new OperateResult<int, byte[]>(StringResources.Language.CommandHeadCodeCheckFailed);
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToInt32(operateResult.Content1, 4), operateResult.Content2);
	}

	private OperateResult<long> ReceiveLong(Socket socket)
	{
		OperateResult<byte[]> operateResult = Receive(socket, 8, -1);
		if (operateResult.IsSuccess)
		{
			return OperateResult.CreateSuccessResult(BitConverter.ToInt64(operateResult.Content, 0));
		}
		return OperateResult.CreateFailedResult<long>(operateResult);
	}

	private OperateResult SendLong(Socket socket, long value)
	{
		return Send(socket, BitConverter.GetBytes(value));
	}

	protected OperateResult SendStreamToSocket(Socket socket, Stream stream, long receive, Action<long, long> report, bool reportByPercent)
	{
		byte[] array = new byte[fileCacheSize];
		long num = 0L;
		long num2 = 0L;
		stream.Position = 0L;
		while (num < receive)
		{
			OperateResult<int> operateResult = NetSupport.ReadStream(stream, array);
			if (!operateResult.IsSuccess)
			{
				socket?.Close();
				return operateResult;
			}
			num += operateResult.Content;
			byte[] array2 = new byte[operateResult.Content];
			Array.Copy(array, 0, array2, 0, array2.Length);
			OperateResult operateResult2 = SendBytesAndCheckReceive(socket, operateResult.Content, array2);
			if (!operateResult2.IsSuccess)
			{
				socket?.Close();
				return operateResult2;
			}
			if (reportByPercent)
			{
				long num3 = num * 100 / receive;
				if (num2 != num3)
				{
					num2 = num3;
					report?.Invoke(num, receive);
				}
			}
			else
			{
				report?.Invoke(num, receive);
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected OperateResult WriteStreamFromSocket(Socket socket, Stream stream, long totalLength, Action<long, long> report, bool reportByPercent)
	{
		long num = 0L;
		long num2 = 0L;
		while (num < totalLength)
		{
			OperateResult<int, byte[]> operateResult = ReceiveBytesContentFromSocket(socket, 60000);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			num += operateResult.Content1;
			OperateResult operateResult2 = NetSupport.WriteStream(stream, operateResult.Content2);
			if (!operateResult2.IsSuccess)
			{
				socket?.Close();
				return operateResult2;
			}
			if (reportByPercent)
			{
				long num3 = num * 100 / totalLength;
				if (num2 != num3)
				{
					num2 = num3;
					report?.Invoke(num, totalLength);
				}
			}
			else
			{
				report?.Invoke(num, totalLength);
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected async Task<OperateResult<Socket>> CreateSocketAndConnectAsync(IPEndPoint endPoint, int timeOut, IPEndPoint local = null)
	{
		OperateResult<Socket> connect = await NetSupport.CreateSocketAndConnectAsync(endPoint, timeOut, local).ConfigureAwait(continueOnCapturedContext: false);
		if (connect.IsSuccess)
		{
			connectErrorCount = 0;
			return connect;
		}
		if (connectErrorCount < 1000000000)
		{
			connectErrorCount++;
		}
		return new OperateResult<Socket>(-connectErrorCount, connect.Message);
	}

	protected async Task<OperateResult<Socket>> CreateSocketAndConnectAsync(string ipAddress, int port)
	{
		return await CreateSocketAndConnectAsync(new IPEndPoint(IPAddress.Parse(ipAddress), port), 10000).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult<Socket>> CreateSocketAndConnectAsync(string ipAddress, int port, int timeOut)
	{
		return await CreateSocketAndConnectAsync(new IPEndPoint(IPAddress.Parse(ipAddress), port), timeOut).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult<byte[]>> ReceiveAsync(Socket socket, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		int bufferLength = ((length > 0) ? length : 2048);
		byte[] buffer;
		try
		{
			buffer = new byte[bufferLength];
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<byte[]>($"Create byte[{bufferLength}] buffer failed: " + ex.Message);
		}
		OperateResult<int> receive = await ReceiveAsync(socket, buffer, 0, length, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(receive);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? buffer : buffer.SelectBegin(receive.Content));
	}

	protected async Task<OperateResult<int>> ReceiveAsync(Socket socket, byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
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
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			hslTimeOut.IsSuccessful = true;
			return new OperateResult<int>(-connectErrorCount, StringResources.Language.RemoteClosedConnection);
		}
		catch (Exception ex2)
		{
			socket?.Close();
			hslTimeOut.IsSuccessful = true;
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			if (hslTimeOut.IsTimeout)
			{
				return new OperateResult<int>(-connectErrorCount, StringResources.Language.ReceiveDataTimeout + hslTimeOut.DelayTime);
			}
			return new OperateResult<int>(-connectErrorCount, "Socket Exception -> " + ex2.Message);
		}
	}

	protected async Task<OperateResult<byte[]>> ReceiveAsync(SslStream ssl, int length, int timeOut, Action<long, long> reportProgress = null)
	{
		if (length == 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		int bufferLength = ((length > 0) ? length : 2048);
		byte[] buffer;
		try
		{
			buffer = new byte[bufferLength];
		}
		catch (Exception ex)
		{
			ssl?.Close();
			return new OperateResult<byte[]>($"Create byte[{bufferLength}] buffer failed: " + ex.Message);
		}
		OperateResult<int> receive = await ReceiveAsync(ssl, buffer, 0, length, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(receive);
		}
		return OperateResult.CreateSuccessResult((length > 0) ? buffer : buffer.SelectBegin(receive.Content));
	}

	protected async Task<OperateResult<int>> ReceiveAsync(SslStream ssl, byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
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
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<int>(-connectErrorCount, StringResources.Language.RemoteClosedConnection);
		}
		catch (Exception ex2)
		{
			ssl?.Close();
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<int>(-connectErrorCount, "Socket Exception -> " + ex2.Message);
		}
	}

	protected async Task<OperateResult<byte[]>> ReceiveCommandLineFromSocketAsync(Socket socket, byte endCode, int timeout = int.MaxValue)
	{
		List<byte> bufferArray = new List<byte>(128);
		try
		{
			DateTime st = DateTime.Now;
			bool bOK = false;
			while ((DateTime.Now - st).TotalMilliseconds < (double)timeout)
			{
				if (socket.Poll(timeout, SelectMode.SelectRead))
				{
					OperateResult<byte[]> headResult = await ReceiveAsync(socket, 1, timeout).ConfigureAwait(continueOnCapturedContext: false);
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
			}
			if (!bOK)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout);
			}
			return OperateResult.CreateSuccessResult(bufferArray.ToArray());
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			socket?.Close();
			return new OperateResult<byte[]>(ex3.Message);
		}
	}

	protected async Task<OperateResult<byte[]>> ReceiveCommandLineFromSocketAsync(Socket socket, byte endCode1, byte endCode2, int timeout = 60000)
	{
		List<byte> bufferArray = new List<byte>(128);
		try
		{
			DateTime st = DateTime.Now;
			bool bOK = false;
			while ((DateTime.Now - st).TotalMilliseconds < (double)timeout)
			{
				if (socket.Poll(timeout, SelectMode.SelectRead))
				{
					OperateResult<byte[]> headResult = await ReceiveAsync(socket, 1, timeout).ConfigureAwait(continueOnCapturedContext: false);
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
			}
			if (!bOK)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout);
			}
			return OperateResult.CreateSuccessResult(bufferArray.ToArray());
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			socket?.Close();
			return new OperateResult<byte[]>(ex3.Message);
		}
	}

	protected async Task<OperateResult> SendAsync(Socket socket, byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return await SendAsync(socket, data, 0, data.Length).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult> SendAsync(Socket socket, byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult(StringResources.Language.AuthorizationFailed);
		//}
		int alreadyCount = 0;
		try
		{
			do
			{
				int count = await Task.Factory.FromAsync(socket.BeginSend(data, offset, size - alreadyCount, SocketFlags.None, null, socket), (Func<IAsyncResult, int>)socket.EndSend).ConfigureAwait(continueOnCapturedContext: false);
				alreadyCount += count;
				offset += count;
			}
			while (alreadyCount < size);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			socket?.Close();
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<byte[]>(-connectErrorCount, ex.Message);
		}
	}

	protected async Task<OperateResult> SendAsync(SslStream ssl, byte[] data)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return await SendAsync(ssl, data, 0, data.Length).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult> SendAsync(SslStream ssl, byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		//if (!Authorization.nzugaydgwadawdibbas())
		//{
		//	return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		//}
		try
		{
			await ssl.WriteAsync(data, offset, size).ConfigureAwait(continueOnCapturedContext: false);
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			ssl?.Close();
			if (connectErrorCount < 1000000000)
			{
				connectErrorCount++;
			}
			return new OperateResult<byte[]>(-connectErrorCount, ex.Message);
		}
	}

	protected virtual async Task<OperateResult<byte[]>> ReceiveByMessageAsync(Socket socket, int timeOut, INetMessage netMessage, Action<long, long> reportProgress = null)
	{
		if (netMessage == null)
		{
			return await ReceiveAsync(socket, -1, timeOut).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (netMessage.ProtocolHeadBytesLength < 0)
		{
			byte[] headCode = BitConverter.GetBytes(netMessage.ProtocolHeadBytesLength);
			int codeLength = headCode[3] & 0xF;
			OperateResult<byte[]> receive = null;
			switch (codeLength)
			{
			case 1:
				receive = await ReceiveCommandLineFromSocketAsync(socket, headCode[1], timeOut).ConfigureAwait(continueOnCapturedContext: false);
				break;
			case 2:
				receive = await ReceiveCommandLineFromSocketAsync(socket, headCode[1], headCode[0], timeOut).ConfigureAwait(continueOnCapturedContext: false);
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
				OperateResult<byte[]> endResult = await ReceiveAsync(socket, message.EndLength, timeOut).ConfigureAwait(continueOnCapturedContext: false);
				if (!endResult.IsSuccess)
				{
					return endResult;
				}
				return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<byte>(receive.Content, endResult.Content));
			}
			return receive;
		}
		OperateResult<byte[]> headResult = await ReceiveAsync(socket, netMessage.ProtocolHeadBytesLength, timeOut).ConfigureAwait(continueOnCapturedContext: false);
		if (!headResult.IsSuccess)
		{
			return headResult;
		}
		int start = netMessage.PependedUselesByteLength(headResult.Content);
		int cycleCount = 0;
		while (start >= netMessage.ProtocolHeadBytesLength)
		{
			headResult = await ReceiveAsync(socket, netMessage.ProtocolHeadBytesLength, timeOut).ConfigureAwait(continueOnCapturedContext: false);
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
			OperateResult<byte[]> head2Result = await ReceiveAsync(socket, start, timeOut).ConfigureAwait(continueOnCapturedContext: false);
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
		byte[] buffer = new byte[netMessage.ProtocolHeadBytesLength + contentLength];
		headResult.Content.CopyTo(buffer, 0);
		OperateResult<int> contentResult = await ReceiveAsync(socket, buffer, netMessage.ProtocolHeadBytesLength, contentLength, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (!contentResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(contentResult);
		}
		return OperateResult.CreateSuccessResult(buffer);
	}

	private async Task<OperateResult<long>> ReceiveLongAsync(Socket socket)
	{
		OperateResult<byte[]> read = await ReceiveAsync(socket, 8, -1).ConfigureAwait(continueOnCapturedContext: false);
		if (read.IsSuccess)
		{
			return OperateResult.CreateSuccessResult(BitConverter.ToInt64(read.Content, 0));
		}
		return OperateResult.CreateFailedResult<long>(read);
	}

	private async Task<OperateResult> SendLongAsync(Socket socket, long value)
	{
		return await SendAsync(socket, BitConverter.GetBytes(value)).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult> SendBaseAndCheckReceiveAsync(Socket socket, int headCode, int customer, byte[] send)
	{
		send = HslProtocol.CommandBytes(headCode, customer, Token, send);
		OperateResult sendResult = await SendAsync(socket, send).ConfigureAwait(continueOnCapturedContext: false);
		if (!sendResult.IsSuccess)
		{
			return sendResult;
		}
		OperateResult<long> checkResult = await ReceiveLongAsync(socket).ConfigureAwait(continueOnCapturedContext: false);
		if (!checkResult.IsSuccess)
		{
			return checkResult;
		}
		if (checkResult.Content != send.Length)
		{
			socket?.Close();
			return new OperateResult(StringResources.Language.CommandLengthCheckFailed);
		}
		return checkResult;
	}

	protected async Task<OperateResult> SendBytesAndCheckReceiveAsync(Socket socket, int customer, byte[] send)
	{
		return await SendBaseAndCheckReceiveAsync(socket, 1002, customer, send).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult> SendStringAndCheckReceiveAsync(Socket socket, int customer, string send)
	{
		byte[] data = (string.IsNullOrEmpty(send) ? null : Encoding.Unicode.GetBytes(send));
		return await SendBaseAndCheckReceiveAsync(socket, 1001, customer, data).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult> SendStringAndCheckReceiveAsync(Socket socket, int customer, string[] sends)
	{
		return await SendBaseAndCheckReceiveAsync(socket, 1005, customer, HslProtocol.PackStringArrayToByte(sends)).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult> SendAccountAndCheckReceiveAsync(Socket socket, int customer, string name, string pwd)
	{
		return await SendBaseAndCheckReceiveAsync(socket, 5, customer, HslProtocol.PackStringArrayToByte(new string[2] { name, pwd })).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult<byte[], byte[]>> ReceiveAndCheckBytesAsync(Socket socket, int timeout)
	{
		OperateResult<byte[]> headResult = await ReceiveAsync(socket, 32, timeout).ConfigureAwait(continueOnCapturedContext: false);
		if (!headResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], byte[]>(headResult);
		}
		if (!CheckRemoteToken(headResult.Content))
		{
			socket?.Close();
			return new OperateResult<byte[], byte[]>(StringResources.Language.TokenCheckFailed);
		}
		int contentLength = BitConverter.ToInt32(headResult.Content, 28);
		OperateResult<byte[]> contentResult = await ReceiveAsync(socket, contentLength, timeout).ConfigureAwait(continueOnCapturedContext: false);
		if (!contentResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], byte[]>(contentResult);
		}
		OperateResult checkResult = await SendLongAsync(socket, 32 + contentLength).ConfigureAwait(continueOnCapturedContext: false);
		if (!checkResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], byte[]>(checkResult);
		}
		byte[] head = headResult.Content;
		byte[] content2 = contentResult.Content;
		content2 = HslProtocol.CommandAnalysis(head, content2);
		return OperateResult.CreateSuccessResult(head, content2);
	}

	protected async Task<OperateResult<int, string>> ReceiveStringContentFromSocketAsync(Socket socket, int timeOut = 30000)
	{
		OperateResult<byte[], byte[]> receive = await ReceiveAndCheckBytesAsync(socket, timeOut).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, string>(receive);
		}
		if (BitConverter.ToInt32(receive.Content1, 0) != 1001)
		{
			socket?.Close();
			return new OperateResult<int, string>(StringResources.Language.CommandHeadCodeCheckFailed);
		}
		if (receive.Content2 == null)
		{
			receive.Content2 = new byte[0];
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToInt32(receive.Content1, 4), Encoding.Unicode.GetString(receive.Content2));
	}

	protected async Task<OperateResult<int, string[]>> ReceiveStringArrayContentFromSocketAsync(Socket socket, int timeOut = 30000)
	{
		OperateResult<byte[], byte[]> receive = await ReceiveAndCheckBytesAsync(socket, timeOut).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, string[]>(receive);
		}
		if (BitConverter.ToInt32(receive.Content1, 0) != 1005)
		{
			socket?.Close();
			return new OperateResult<int, string[]>(StringResources.Language.CommandHeadCodeCheckFailed);
		}
		if (receive.Content2 == null)
		{
			receive.Content2 = new byte[4];
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToInt32(receive.Content1, 4), HslProtocol.UnPackStringArrayFromByte(receive.Content2));
	}

	protected async Task<OperateResult<int, byte[]>> ReceiveBytesContentFromSocketAsync(Socket socket, int timeout = 30000)
	{
		OperateResult<byte[], byte[]> receive = await ReceiveAndCheckBytesAsync(socket, timeout).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, byte[]>(receive);
		}
		if (BitConverter.ToInt32(receive.Content1, 0) != 1002)
		{
			socket?.Close();
			return new OperateResult<int, byte[]>(StringResources.Language.CommandHeadCodeCheckFailed);
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToInt32(receive.Content1, 4), receive.Content2);
	}

	protected async Task<OperateResult> SendStreamToSocketAsync(Socket socket, Stream stream, long receive, Action<long, long> report, bool reportByPercent)
	{
		byte[] buffer = new byte[fileCacheSize];
		long SendTotal = 0L;
		long percent = 0L;
		stream.Position = 0L;
		while (SendTotal < receive)
		{
			OperateResult<int> read = await NetSupport.ReadStreamAsync(stream, buffer).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				socket?.Close();
				return read;
			}
			SendTotal += read.Content;
			byte[] newBuffer = new byte[read.Content];
			Array.Copy(buffer, 0, newBuffer, 0, newBuffer.Length);
			OperateResult write = await SendBytesAndCheckReceiveAsync(socket, read.Content, newBuffer).ConfigureAwait(continueOnCapturedContext: false);
			if (!write.IsSuccess)
			{
				socket?.Close();
				return write;
			}
			if (reportByPercent)
			{
				long percentCurrent = SendTotal * 100 / receive;
				if (percent != percentCurrent)
				{
					percent = percentCurrent;
					report?.Invoke(SendTotal, receive);
				}
			}
			else
			{
				report?.Invoke(SendTotal, receive);
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected async Task<OperateResult> WriteStreamFromSocketAsync(Socket socket, Stream stream, long totalLength, Action<long, long> report, bool reportByPercent)
	{
		long count_receive = 0L;
		long percent = 0L;
		while (count_receive < totalLength)
		{
			OperateResult<int, byte[]> read = await ReceiveBytesContentFromSocketAsync(socket, 60000).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				return read;
			}
			count_receive += read.Content1;
			OperateResult write = await NetSupport.WriteStreamAsync(stream, read.Content2).ConfigureAwait(continueOnCapturedContext: false);
			if (!write.IsSuccess)
			{
				socket?.Close();
				return write;
			}
			if (reportByPercent)
			{
				long percentCurrent = count_receive * 100 / totalLength;
				if (percent != percentCurrent)
				{
					percent = percentCurrent;
					report?.Invoke(count_receive, totalLength);
				}
			}
			else
			{
				report?.Invoke(count_receive, totalLength);
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected OperateResult<byte, byte[]> ReceiveMqttMessage(Socket socket, int timeOut, Action<long, long> reportProgress = null)
	{
		return MqttHelper.ReceiveMqttMessage(Receive, socket, timeOut, reportProgress);
	}

	protected OperateResult<byte, byte[]> ReceiveMqttMessage(SslStream ssl, int timeOut, Action<long, long> reportProgress = null)
	{
		return MqttHelper.ReceiveMqttMessage(Receive, ssl, timeOut, reportProgress);
	}

	protected OperateResult ReceiveMqttStream(Socket socket, Stream stream, long fileSize, int timeOut, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		long num = 0L;
		while (num < fileSize)
		{
			OperateResult<byte, byte[]> operateResult = ReceiveMqttMessage(socket, timeOut);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			if (operateResult.Content1 == 0)
			{
				socket?.Close();
				return new OperateResult(Encoding.UTF8.GetString(operateResult.Content2));
			}
			if (aesCryptography != null)
			{
				try
				{
					operateResult.Content2 = aesCryptography.Decrypt(operateResult.Content2);
				}
				catch (Exception ex)
				{
					socket?.Close();
					return new OperateResult("AES Decrypt file stream failed: " + ex.Message);
				}
			}
			OperateResult operateResult2 = NetSupport.WriteStream(stream, operateResult.Content2);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			num += operateResult.Content2.Length;
			byte[] array = new byte[16];
			BitConverter.GetBytes(num).CopyTo(array, 0);
			BitConverter.GetBytes(fileSize).CopyTo(array, 8);
			if (cancelToken != null && cancelToken.IsCancelled)
			{
				OperateResult operateResult3 = Send(socket, MqttHelper.BuildMqttCommand(0, null, HslHelper.GetUTF8Bytes(StringResources.Language.UserCancelOperate)).Content);
				if (!operateResult3.IsSuccess)
				{
					socket?.Close();
					return operateResult3;
				}
				socket?.Close();
				return new OperateResult(StringResources.Language.UserCancelOperate);
			}
			OperateResult operateResult4 = Send(socket, MqttHelper.BuildMqttCommand(100, null, array).Content);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			reportProgress?.Invoke(num, fileSize);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected OperateResult SendMqttStream(Socket socket, Stream stream, long fileSize, int timeOut, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		byte[] array = new byte[fileCacheSize];
		long num = 0L;
		stream.Position = 0L;
		while (num < fileSize)
		{
			OperateResult<int> operateResult = NetSupport.ReadStream(stream, array);
			if (!operateResult.IsSuccess)
			{
				socket?.Close();
				return operateResult;
			}
			num += operateResult.Content;
			if (cancelToken != null && cancelToken.IsCancelled)
			{
				OperateResult operateResult2 = Send(socket, MqttHelper.BuildMqttCommand(0, null, HslHelper.GetUTF8Bytes(StringResources.Language.UserCancelOperate)).Content);
				if (!operateResult2.IsSuccess)
				{
					socket?.Close();
					return operateResult2;
				}
				socket?.Close();
				return new OperateResult(StringResources.Language.UserCancelOperate);
			}
			OperateResult operateResult3 = Send(socket, MqttHelper.BuildMqttCommand(100, null, array.SelectBegin(operateResult.Content), aesCryptography).Content);
			if (!operateResult3.IsSuccess)
			{
				socket?.Close();
				return operateResult3;
			}
			OperateResult<byte, byte[]> operateResult4 = ReceiveMqttMessage(socket, timeOut);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			if (operateResult4.Content1 == 0)
			{
				socket?.Close();
				return new OperateResult(Encoding.UTF8.GetString(operateResult4.Content2));
			}
			reportProgress?.Invoke(num, fileSize);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected OperateResult SendMqttFile(Socket socket, string filename, string servername, string filetag, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		FileInfo fileInfo = new FileInfo(filename);
		if (!File.Exists(filename))
		{
			OperateResult operateResult = Send(socket, MqttHelper.BuildMqttCommand(0, null, Encoding.UTF8.GetBytes(StringResources.Language.FileNotExist)).Content);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			socket?.Close();
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		string[] data = new string[3]
		{
			servername,
			fileInfo.Length.ToString(),
			filetag
		};
		OperateResult operateResult2 = Send(socket, MqttHelper.BuildMqttCommand(100, null, HslProtocol.PackStringArrayToByte(data)).Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult<byte, byte[]> operateResult3 = ReceiveMqttMessage(socket, 60000);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		if (operateResult3.Content1 == 0)
		{
			socket?.Close();
			return new OperateResult(Encoding.UTF8.GetString(operateResult3.Content2));
		}
		try
		{
			OperateResult result = new OperateResult();
			using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				result = SendMqttStream(socket, stream, fileInfo.Length, 60000, reportProgress, aesCryptography, cancelToken);
			}
			return result;
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult("SendMqttStream Exception -> " + ex.Message);
		}
	}

	protected OperateResult SendMqttFile(Socket socket, Stream stream, string servername, string filetag, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		string[] data = new string[3]
		{
			servername,
			stream.Length.ToString(),
			filetag
		};
		OperateResult operateResult = Send(socket, MqttHelper.BuildMqttCommand(100, null, HslProtocol.PackStringArrayToByte(data)).Content);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte, byte[]> operateResult2 = ReceiveMqttMessage(socket, 60000);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if (operateResult2.Content1 == 0)
		{
			socket?.Close();
			return new OperateResult(Encoding.UTF8.GetString(operateResult2.Content2));
		}
		try
		{
			return SendMqttStream(socket, stream, stream.Length, 60000, reportProgress, aesCryptography, cancelToken);
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult("SendMqttStream Exception -> " + ex.Message);
		}
	}

	protected OperateResult<FileBaseInfo> ReceiveMqttFile(Socket socket, object source, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		OperateResult<byte, byte[]> operateResult = ReceiveMqttMessage(socket, 60000);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(operateResult);
		}
		if (operateResult.Content1 == 0)
		{
			socket?.Close();
			return new OperateResult<FileBaseInfo>(Encoding.UTF8.GetString(operateResult.Content2));
		}
		FileBaseInfo fileBaseInfo = new FileBaseInfo();
		string[] array = HslProtocol.UnPackStringArrayFromByte(operateResult.Content2);
		fileBaseInfo.Name = array[0];
		fileBaseInfo.Size = long.Parse(array[1]);
		fileBaseInfo.Tag = array[2];
		Send(socket, MqttHelper.BuildMqttCommand(100, null, null).Content);
		try
		{
			OperateResult operateResult2 = null;
			if (source is string path)
			{
				using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
				{
					operateResult2 = ReceiveMqttStream(socket, stream, fileBaseInfo.Size, 60000, reportProgress, aesCryptography, cancelToken);
				}
				if (!operateResult2.IsSuccess)
				{
					if (File.Exists(path))
					{
						File.Delete(path);
					}
					return OperateResult.CreateFailedResult<FileBaseInfo>(operateResult2);
				}
			}
			else
			{
				if (!(source is Stream stream2))
				{
					throw new Exception("Not Supported Type");
				}
				operateResult2 = ReceiveMqttStream(socket, stream2, fileBaseInfo.Size, 60000, reportProgress, aesCryptography, cancelToken);
			}
			return OperateResult.CreateSuccessResult(fileBaseInfo);
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<FileBaseInfo>(ex.Message);
		}
	}

	protected async Task<OperateResult<byte, byte[]>> ReceiveMqttMessageAsync(Socket socket, int timeOut, Action<long, long> reportProgress = null)
	{
		return await MqttHelper.ReceiveMqttMessageAsync(ReceiveAsync, socket, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult<byte, byte[]>> ReceiveMqttMessageAsync(SslStream ssl, int timeOut, Action<long, long> reportProgress = null)
	{
		return await MqttHelper.ReceiveMqttMessageAsync(ReceiveAsync, ssl, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected async Task<OperateResult> ReceiveMqttStreamAsync(Socket socket, Stream stream, long fileSize, int timeOut, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		long already = 0L;
		while (already < fileSize)
		{
			OperateResult<byte, byte[]> receive = await ReceiveMqttMessageAsync(socket, timeOut).ConfigureAwait(continueOnCapturedContext: false);
			if (!receive.IsSuccess)
			{
				return receive;
			}
			if (receive.Content1 == 0)
			{
				socket?.Close();
				return new OperateResult(Encoding.UTF8.GetString(receive.Content2));
			}
			if (aesCryptography != null)
			{
				try
				{
					receive.Content2 = aesCryptography.Decrypt(receive.Content2);
				}
				catch (Exception ex)
				{
					Exception ex2 = ex;
					Exception ex3 = ex2;
					socket?.Close();
					return new OperateResult("AES Decrypt file stream failed: " + ex3.Message);
				}
			}
			OperateResult write = await NetSupport.WriteStreamAsync(stream, receive.Content2).ConfigureAwait(continueOnCapturedContext: false);
			if (!write.IsSuccess)
			{
				return write;
			}
			already += receive.Content2.Length;
			byte[] ack = new byte[16];
			BitConverter.GetBytes(already).CopyTo(ack, 0);
			BitConverter.GetBytes(fileSize).CopyTo(ack, 8);
			if (cancelToken?.IsCancelled ?? false)
			{
				OperateResult cancel = Send(socket, MqttHelper.BuildMqttCommand(0, null, HslHelper.GetUTF8Bytes(StringResources.Language.UserCancelOperate)).Content);
				if (!cancel.IsSuccess)
				{
					socket?.Close();
					return cancel;
				}
				socket?.Close();
				return new OperateResult(StringResources.Language.UserCancelOperate);
			}
			OperateResult send = await SendAsync(socket, MqttHelper.BuildMqttCommand(100, null, ack).Content).ConfigureAwait(continueOnCapturedContext: false);
			if (!send.IsSuccess)
			{
				return send;
			}
			reportProgress?.Invoke(already, fileSize);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected async Task<OperateResult> SendMqttStreamAsync(Socket socket, Stream stream, long fileSize, int timeOut, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		byte[] buffer = new byte[fileCacheSize];
		long already = 0L;
		stream.Position = 0L;
		while (already < fileSize)
		{
			OperateResult<int> read = await NetSupport.ReadStreamAsync(stream, buffer).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				socket?.Close();
				return read;
			}
			if (cancelToken?.IsCancelled ?? false)
			{
				OperateResult cancel = await SendAsync(socket, MqttHelper.BuildMqttCommand(0, null, HslHelper.GetUTF8Bytes(StringResources.Language.UserCancelOperate)).Content).ConfigureAwait(continueOnCapturedContext: false);
				if (!cancel.IsSuccess)
				{
					socket?.Close();
					return cancel;
				}
				socket?.Close();
				return new OperateResult(StringResources.Language.UserCancelOperate);
			}
			already += read.Content;
			OperateResult write = await SendAsync(socket, MqttHelper.BuildMqttCommand(100, null, buffer.SelectBegin(read.Content), aesCryptography).Content).ConfigureAwait(continueOnCapturedContext: false);
			if (!write.IsSuccess)
			{
				socket?.Close();
				return write;
			}
			OperateResult<byte, byte[]> receive = await ReceiveMqttMessageAsync(socket, timeOut).ConfigureAwait(continueOnCapturedContext: false);
			if (!receive.IsSuccess)
			{
				return receive;
			}
			if (receive.Content1 == 0)
			{
				socket?.Close();
				return new OperateResult(Encoding.UTF8.GetString(receive.Content2));
			}
			reportProgress?.Invoke(already, fileSize);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected async Task<OperateResult> SendMqttFileAsync(Socket socket, string filename, string servername, string filetag, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		FileInfo info = new FileInfo(filename);
		if (!File.Exists(filename))
		{
			OperateResult notFoundResult = await SendAsync(socket, MqttHelper.BuildMqttCommand(0, null, Encoding.UTF8.GetBytes(StringResources.Language.FileNotExist)).Content).ConfigureAwait(continueOnCapturedContext: false);
			if (!notFoundResult.IsSuccess)
			{
				return notFoundResult;
			}
			socket?.Close();
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		string[] array = new string[3]
		{
			servername,
			info.Length.ToString(),
			filetag
		};
		OperateResult sendResult = await SendAsync(socket, MqttHelper.BuildMqttCommand(100, null, HslProtocol.PackStringArrayToByte(array)).Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!sendResult.IsSuccess)
		{
			return sendResult;
		}
		OperateResult<byte, byte[]> check = await ReceiveMqttMessageAsync(socket, 60000).ConfigureAwait(continueOnCapturedContext: false);
		if (!check.IsSuccess)
		{
			return check;
		}
		if (check.Content1 == 0)
		{
			socket?.Close();
			return new OperateResult(Encoding.UTF8.GetString(check.Content2));
		}
		try
		{
			OperateResult result = new OperateResult();
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				result = await SendMqttStreamAsync(socket, fs, info.Length, 60000, reportProgress, aesCryptography, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			return result;
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult("SendMqttStreamAsync Exception -> " + ex.Message);
		}
	}

	protected async Task<OperateResult> SendMqttFileAsync(Socket socket, Stream stream, string servername, string filetag, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		string[] array = new string[3]
		{
			servername,
			stream.Length.ToString(),
			filetag
		};
		OperateResult sendResult = await SendAsync(socket, MqttHelper.BuildMqttCommand(100, null, HslProtocol.PackStringArrayToByte(array)).Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!sendResult.IsSuccess)
		{
			return sendResult;
		}
		OperateResult<byte, byte[]> check = await ReceiveMqttMessageAsync(socket, 60000).ConfigureAwait(continueOnCapturedContext: false);
		if (!check.IsSuccess)
		{
			return check;
		}
		if (check.Content1 == 0)
		{
			socket?.Close();
			return new OperateResult(Encoding.UTF8.GetString(check.Content2));
		}
		try
		{
			return await SendMqttStreamAsync(socket, stream, stream.Length, 60000, reportProgress, aesCryptography, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult("SendMqttStreamAsync Exception -> " + ex.Message);
		}
	}

	protected async Task<OperateResult<FileBaseInfo>> ReceiveMqttFileAsync(Socket socket, object source, Action<long, long> reportProgress = null, AesCryptography aesCryptography = null, HslCancelToken cancelToken = null)
	{
		OperateResult<byte, byte[]> receiveFileInfo = await ReceiveMqttMessageAsync(socket, 60000).ConfigureAwait(continueOnCapturedContext: false);
		if (!receiveFileInfo.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(receiveFileInfo);
		}
		if (receiveFileInfo.Content1 == 0)
		{
			socket?.Close();
			return new OperateResult<FileBaseInfo>(Encoding.UTF8.GetString(receiveFileInfo.Content2));
		}
		FileBaseInfo fileBaseInfo = new FileBaseInfo();
		string[] array = HslProtocol.UnPackStringArrayFromByte(receiveFileInfo.Content2);
		if (array.Length < 3)
		{
			socket?.Close();
			return new OperateResult<FileBaseInfo>("FileBaseInfo Check failed: " + array.ToArrayString());
		}
		fileBaseInfo.Name = array[0];
		fileBaseInfo.Size = long.Parse(array[1]);
		fileBaseInfo.Tag = array[2];
		await SendAsync(socket, MqttHelper.BuildMqttCommand(100, null, null).Content).ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			OperateResult write = null;
			if (source is string savename)
			{
				using (FileStream fs = new FileStream(savename, FileMode.Create, FileAccess.Write))
				{
					write = await ReceiveMqttStreamAsync(socket, fs, fileBaseInfo.Size, 60000, reportProgress, aesCryptography, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				if (!write.IsSuccess)
				{
					if (File.Exists(savename))
					{
						File.Delete(savename);
					}
					return OperateResult.CreateFailedResult<FileBaseInfo>(write);
				}
			}
			else
			{
				if (!(source is Stream stream))
				{
					throw new Exception("Not Supported Type");
				}
				await ReceiveMqttStreamAsync(socket, stream, fileBaseInfo.Size, 60000, reportProgress, aesCryptography, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			return OperateResult.CreateSuccessResult(fileBaseInfo);
		}
		catch (Exception ex)
		{
			socket?.Close();
			return new OperateResult<FileBaseInfo>(ex.Message);
		}
	}

	protected OperateResult<byte[]> ReceiveRedisCommandString(Socket socket, int length)
	{
		List<byte> list = new List<byte>();
		OperateResult<byte[]> operateResult = Receive(socket, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		list.AddRange(operateResult.Content);
		OperateResult<byte[]> operateResult2 = ReceiveCommandLineFromSocket(socket, 10);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		list.AddRange(operateResult2.Content);
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	protected OperateResult<byte[]> ReceiveRedisCommand(Socket socket)
	{
		List<byte> list = new List<byte>();
		OperateResult<byte[]> operateResult = ReceiveCommandLineFromSocket(socket, 10);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		list.AddRange(operateResult.Content);
		if (operateResult.Content[0] == 43 || operateResult.Content[0] == 45 || operateResult.Content[0] == 58)
		{
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		if (operateResult.Content[0] == 36)
		{
			OperateResult<int> numberFromCommandLine = RedisHelper.GetNumberFromCommandLine(operateResult.Content);
			if (!numberFromCommandLine.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(numberFromCommandLine);
			}
			if (numberFromCommandLine.Content < 0)
			{
				return OperateResult.CreateSuccessResult(list.ToArray());
			}
			OperateResult<byte[]> operateResult2 = ReceiveRedisCommandString(socket, numberFromCommandLine.Content);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			list.AddRange(operateResult2.Content);
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		if (operateResult.Content[0] == 42)
		{
			OperateResult<int> numberFromCommandLine2 = RedisHelper.GetNumberFromCommandLine(operateResult.Content);
			if (!numberFromCommandLine2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(numberFromCommandLine2);
			}
			for (int i = 0; i < numberFromCommandLine2.Content; i++)
			{
				OperateResult<byte[]> operateResult3 = ReceiveRedisCommand(socket);
				if (!operateResult3.IsSuccess)
				{
					return operateResult3;
				}
				list.AddRange(operateResult3.Content);
			}
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		return new OperateResult<byte[]>("Not Supported HeadCode: " + operateResult.Content[0]);
	}

	protected async Task<OperateResult<byte[]>> ReceiveRedisCommandStringAsync(Socket socket, int length)
	{
		List<byte> bufferArray = new List<byte>();
		OperateResult<byte[]> receive = await ReceiveAsync(socket, length).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			return receive;
		}
		bufferArray.AddRange(receive.Content);
		OperateResult<byte[]> commandTail = await ReceiveCommandLineFromSocketAsync(socket, 10).ConfigureAwait(continueOnCapturedContext: false);
		if (!commandTail.IsSuccess)
		{
			return commandTail;
		}
		bufferArray.AddRange(commandTail.Content);
		return OperateResult.CreateSuccessResult(bufferArray.ToArray());
	}

	protected async Task<OperateResult<byte[]>> ReceiveRedisCommandAsync(Socket socket)
	{
		List<byte> bufferArray = new List<byte>();
		OperateResult<byte[]> readCommandLine = await ReceiveCommandLineFromSocketAsync(socket, 10).ConfigureAwait(continueOnCapturedContext: false);
		if (!readCommandLine.IsSuccess)
		{
			return readCommandLine;
		}
		bufferArray.AddRange(readCommandLine.Content);
		if (readCommandLine.Content[0] == 43 || readCommandLine.Content[0] == 45 || readCommandLine.Content[0] == 58)
		{
			return OperateResult.CreateSuccessResult(bufferArray.ToArray());
		}
		if (readCommandLine.Content[0] == 36)
		{
			OperateResult<int> lengthResult2 = RedisHelper.GetNumberFromCommandLine(readCommandLine.Content);
			if (!lengthResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(lengthResult2);
			}
			if (lengthResult2.Content < 0)
			{
				return OperateResult.CreateSuccessResult(bufferArray.ToArray());
			}
			OperateResult<byte[]> receiveContent = await ReceiveRedisCommandStringAsync(socket, lengthResult2.Content).ConfigureAwait(continueOnCapturedContext: false);
			if (!receiveContent.IsSuccess)
			{
				return receiveContent;
			}
			bufferArray.AddRange(receiveContent.Content);
			return OperateResult.CreateSuccessResult(bufferArray.ToArray());
		}
		if (readCommandLine.Content[0] == 42)
		{
			OperateResult<int> lengthResult3 = RedisHelper.GetNumberFromCommandLine(readCommandLine.Content);
			if (!lengthResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(lengthResult3);
			}
			for (int i = 0; i < lengthResult3.Content; i++)
			{
				OperateResult<byte[]> receiveCommand = await ReceiveRedisCommandAsync(socket).ConfigureAwait(continueOnCapturedContext: false);
				if (!receiveCommand.IsSuccess)
				{
					return receiveCommand;
				}
				bufferArray.AddRange(receiveCommand.Content);
			}
			return OperateResult.CreateSuccessResult(bufferArray.ToArray());
		}
		return new OperateResult<byte[]>("Not Supported HeadCode: " + readCommandLine.Content[0]);
	}

	protected OperateResult<int, int, byte[]> ReceiveHslMessage(Socket socket)
	{
		OperateResult<byte[]> operateResult = Receive(socket, 32, 10000);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, int, byte[]>(operateResult);
		}
		int length = BitConverter.ToInt32(operateResult.Content, operateResult.Content.Length - 4);
		OperateResult<byte[]> operateResult2 = Receive(socket, length);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, int, byte[]>(operateResult2);
		}
		byte[] value = HslProtocol.CommandAnalysis(operateResult.Content, operateResult2.Content);
		int value2 = BitConverter.ToInt32(operateResult.Content, 0);
		int value3 = BitConverter.ToInt32(operateResult.Content, 4);
		return OperateResult.CreateSuccessResult(value2, value3, value);
	}

	protected async Task<OperateResult<int, int, byte[]>> ReceiveHslMessageAsync(Socket socket)
	{
		OperateResult<byte[]> receiveHead = await ReceiveAsync(socket, 32, 10000).ConfigureAwait(continueOnCapturedContext: false);
		if (!receiveHead.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, int, byte[]>(receiveHead);
		}
		int receive_length = BitConverter.ToInt32(receiveHead.Content, receiveHead.Content.Length - 4);
		OperateResult<byte[]> receiveContent = await ReceiveAsync(socket, receive_length).ConfigureAwait(continueOnCapturedContext: false);
		if (!receiveContent.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int, int, byte[]>(receiveContent);
		}
		byte[] Content = HslProtocol.CommandAnalysis(receiveHead.Content, receiveContent.Content);
		int protocol = BitConverter.ToInt32(receiveHead.Content, 0);
		int customer = BitConverter.ToInt32(receiveHead.Content, 4);
		return OperateResult.CreateSuccessResult(protocol, customer, Content);
	}

	protected bool DeleteFileByName(string fileName)
	{
		try
		{
			if (!File.Exists(fileName))
			{
				return true;
			}
			File.Delete(fileName);
			return true;
		}
		catch (Exception ex)
		{
			LogNet?.WriteException(ToString(), "delete file [" + fileName + "] failed: ", ex);
			return false;
		}
	}

	public override string ToString()
	{
		return "NetworkBase";
	}
}
