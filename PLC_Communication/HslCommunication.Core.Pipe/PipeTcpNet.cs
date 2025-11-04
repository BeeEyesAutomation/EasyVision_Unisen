using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.Core.IMessage;

namespace HslCommunication.Core.Pipe;

public class PipeTcpNet : CommunicationPipe
{
	protected string host = "127.0.0.1";

	private string ipAddress = "127.0.0.1";

	private int[] _port = new int[1] { 2000 };

	private int indexPort = -1;

	private Socket socket;

	private int connectTimeOut = 10000;

	private int recvTimeOutTick = 0;

	private Func<IAsyncResult, int> socketEndMethod;

	public IPEndPoint LocalBinding { get; set; }

	public string IpAddress
	{
		get
		{
			return ipAddress;
		}
		set
		{
			host = value;
			ipAddress = HslHelper.GetIpAddressFromInput(value);
		}
	}

	public string Host => host;

	public int SocketKeepAliveTime { get; set; } = -1;

	public int Port
	{
		get
		{
			if (_port.Length == 1)
			{
				return _port[0];
			}
			int num = indexPort;
			if (num < 0 || num >= _port.Length)
			{
				num = 0;
			}
			return _port[num];
		}
		set
		{
			if (_port.Length == 1)
			{
				_port[0] = value;
				return;
			}
			int num = indexPort;
			if (num < 0 || num >= _port.Length)
			{
				num = 0;
			}
			_port[num] = value;
		}
	}

	public Socket Socket
	{
		get
		{
			return socket;
		}
		set
		{
			socket = value;
			if (socket != null)
			{
				socketEndMethod = socket.EndReceive;
			}
			else
			{
				socketEndMethod = null;
			}
		}
	}

	public int ConnectTimeOut
	{
		get
		{
			return connectTimeOut;
		}
		set
		{
			connectTimeOut = value;
		}
	}

	public int CloseOnRecvTimeOutTick { get; set; } = 1;

	public PipeTcpNet()
	{
	}

	public PipeTcpNet(string ipAddress, int port)
	{
		IpAddress = ipAddress;
		Port = port;
	}

	public PipeTcpNet(Socket socket, IPEndPoint iPEndPoint)
	{
		Socket = socket;
		IpAddress = iPEndPoint.Address.ToString();
		Port = iPEndPoint.Port;
	}

	public void SetMultiPorts(int[] ports)
	{
		if (ports != null && ports.Length != 0)
		{
			_port = ports;
			indexPort = -1;
		}
	}

	public IPEndPoint GetConnectIPEndPoint()
	{
		if (_port.Length == 1)
		{
			return new IPEndPoint(IPAddress.Parse(IpAddress), _port[0]);
		}
		ChangePorts();
		int port = _port[indexPort];
		return new IPEndPoint(IPAddress.Parse(IpAddress), port);
	}

	private void ChangePorts()
	{
		if (_port.Length != 1)
		{
			if (indexPort < _port.Length - 1)
			{
				indexPort++;
			}
			else
			{
				indexPort = 0;
			}
		}
	}

	public override bool IsConnectError()
	{
		if (socket == null)
		{
			return true;
		}
		return base.IsConnectError();
	}

	protected virtual OperateResult OnCommunicationOpen(Socket socket)
	{
		return OperateResult.CreateSuccessResult();
	}

	public override OperateResult<bool> OpenCommunication()
	{
		if (IsConnectError())
		{
			NetSupport.CloseSocket(socket);
			IPEndPoint connectIPEndPoint = GetConnectIPEndPoint();
			OperateResult<Socket> operateResult = NetSupport.CreateSocketAndConnect(connectIPEndPoint, ConnectTimeOut, LocalBinding);
			if (operateResult.IsSuccess)
			{
				OperateResult operateResult2 = OnCommunicationOpen(operateResult.Content);
				if (!operateResult2.IsSuccess)
				{
					return operateResult2.ConvertFailed<bool>();
				}
				if (SocketKeepAliveTime > 0)
				{
					operateResult.Content.SetKeepAlive(SocketKeepAliveTime, SocketKeepAliveTime);
				}
				ResetConnectErrorCount();
				Socket = operateResult.Content;
				return OperateResult.CreateSuccessResult(value: true);
			}
			return new OperateResult<bool>(-IncrConnectErrorCount(), operateResult.Message);
		}
		return OperateResult.CreateSuccessResult(value: false);
	}

	public override OperateResult CloseCommunication()
	{
		NetSupport.CloseSocket(socket);
		Socket = null;
		return OperateResult.CreateSuccessResult();
	}

	public override OperateResult Send(byte[] data, int offset, int size)
	{
		OperateResult operateResult = NetSupport.SocketSend(socket, data, offset, size);
		if (!operateResult.IsSuccess && operateResult.ErrorCode == NetSupport.SocketErrorCode)
		{
			CloseCommunication();
			return new OperateResult<byte[]>(-IncrConnectErrorCount(), operateResult.Message);
		}
		return operateResult;
	}

	public override OperateResult<int> Receive(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		OperateResult<int> operateResult = NetSupport.SocketReceive(socket, buffer, offset, length, timeOut, reportProgress);
		if (!operateResult.IsSuccess && operateResult.ErrorCode == NetSupport.SocketErrorCode)
		{
			if (operateResult.Content == int.MaxValue)
			{
				if (CloseOnRecvTimeOutTick < 0 || Interlocked.Increment(ref recvTimeOutTick) < CloseOnRecvTimeOutTick)
				{
					return new OperateResult<int>(-1, operateResult.Message);
				}
				CloseCommunication();
				Interlocked.Exchange(ref recvTimeOutTick, 0);
			}
			else
			{
				CloseCommunication();
			}
			return new OperateResult<int>(-IncrConnectErrorCount(), operateResult.Message);
		}
		if (operateResult.IsSuccess)
		{
			Interlocked.Exchange(ref recvTimeOutTick, 0);
		}
		return operateResult;
	}

	public override OperateResult StartReceiveBackground(INetMessage netMessage)
	{
		if (base.UseServerActivePush)
		{
			OperateResult operateResult = socket.BeginReceiveResult(ServerSocketActivePushAsync, netMessage);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		return base.StartReceiveBackground(netMessage);
	}

	private async void ServerSocketActivePushAsync(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (!(asyncState is INetMessage netMessage))
		{
			return;
		}
		OperateResult<int> endResult = socket.EndReceiveResult(ar);
		if (!endResult.IsSuccess)
		{
			IncrConnectErrorCount();
			return;
		}
		OperateResult<byte[]> receive = await ReceiveMessageAsync(netMessage, null, useActivePush: false).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess)
		{
			IncrConnectErrorCount();
			return;
		}
		if (base.DecideWhetherQAMessageFunction != null)
		{
			if (base.DecideWhetherQAMessageFunction(this, receive))
			{
				SetBufferQA(receive.Content);
			}
		}
		else
		{
			SetBufferQA(receive.Content);
		}
		OperateResult receiveAgain = socket.BeginReceiveResult(ServerSocketActivePushAsync, netMessage);
		if (!receiveAgain.IsSuccess)
		{
			IncrConnectErrorCount();
		}
	}

	public override async Task<OperateResult<bool>> OpenCommunicationAsync()
	{
		if (IsConnectError())
		{
			NetSupport.CloseSocket(socket);
			IPEndPoint endPoint = GetConnectIPEndPoint();
			OperateResult<Socket> connect = await NetSupport.CreateSocketAndConnectAsync(endPoint, ConnectTimeOut, LocalBinding).ConfigureAwait(continueOnCapturedContext: false);
			if (connect.IsSuccess)
			{
				OperateResult onOpen = OnCommunicationOpen(connect.Content);
				if (!onOpen.IsSuccess)
				{
					return onOpen.ConvertFailed<bool>();
				}
				if (SocketKeepAliveTime > 0)
				{
					connect.Content.SetKeepAlive(SocketKeepAliveTime, SocketKeepAliveTime);
				}
				ResetConnectErrorCount();
				Socket = connect.Content;
				return OperateResult.CreateSuccessResult(value: true);
			}
			return new OperateResult<bool>(-IncrConnectErrorCount(), connect.Message);
		}
		return OperateResult.CreateSuccessResult(value: false);
	}

	public override async Task<OperateResult> CloseCommunicationAsync()
	{
		NetSupport.CloseSocket(socket);
		Socket = null;
		return await Task.FromResult(OperateResult.CreateSuccessResult());
	}

	public override async Task<OperateResult> SendAsync(byte[] data, int offset, int size)
	{
		OperateResult send = await NetSupport.SocketSendAsync(socket, data, offset, size).ConfigureAwait(continueOnCapturedContext: false);
		if (!send.IsSuccess && send.ErrorCode == NetSupport.SocketErrorCode)
		{
			await CloseCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
			return new OperateResult<byte[]>(-IncrConnectErrorCount(), send.Message);
		}
		return send;
	}

	public override async Task<OperateResult<int>> ReceiveAsync(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		Func<IAsyncResult, int> endMethodTmp = socketEndMethod;
		if (endMethodTmp == null && socket != null)
		{
			endMethodTmp = socket.EndReceive;
		}
		OperateResult<int> receive = await NetSupport.SocketReceiveAsync2(socket, buffer, offset, length, timeOut, reportProgress, endMethodTmp).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess && receive.ErrorCode == NetSupport.SocketErrorCode)
		{
			await CloseCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
			return new OperateResult<int>(-IncrConnectErrorCount(), receive.Message);
		}
		return receive;
	}

	public override string ToString()
	{
		return $"PipeTcpNet[{ipAddress}:{Port}]";
	}
}
