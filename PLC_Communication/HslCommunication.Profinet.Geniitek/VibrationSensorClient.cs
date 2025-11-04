using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Geniitek;

public class VibrationSensorClient : NetworkXBase
{
	public delegate void OnPeekValueReceiveDelegate(VibrationSensorPeekValue peekValue);

	public delegate void OnActualValueReceiveDelegate(VibrationSensorActualValue actualValue);

	public delegate void OnClientConnectedDelegate();

	private int isReConnectServer = 0;

	private bool closed = false;

	private string ipAddress = string.Empty;

	private int port = 1883;

	private int connectTimeOut = 10000;

	private Timer timerCheck;

	private DateTime activeTime = DateTime.Now;

	private int checkSeconds = 60;

	private int CheckTimeoutCount = 0;

	private ushort address = 1;

	private IByteTransform byteTransform;

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

	public int CheckSeconds
	{
		get
		{
			return checkSeconds;
		}
		set
		{
			checkSeconds = value;
		}
	}

	public ushort Address
	{
		get
		{
			return address;
		}
		set
		{
			address = value;
		}
	}

	public event OnPeekValueReceiveDelegate OnPeekValueReceive;

	public event OnActualValueReceiveDelegate OnActualValueReceive;

	public event OnClientConnectedDelegate OnClientConnected;

	public event EventHandler OnNetworkError;

	public VibrationSensorClient(string ipAddress = "192.168.1.1", int port = 3001)
	{
		this.ipAddress = HslHelper.GetIpAddressFromInput(ipAddress);
		this.port = port;
		byteTransform = new ReverseBytesTransform();
	}

	public OperateResult ConnectServer()
	{
		CoreSocket?.Close();
		OperateResult<Socket> operateResult = NetSupport.CreateSocketAndConnect(ipAddress, port, connectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		CoreSocket = operateResult.Content;
		try
		{
			CoreSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveAsyncCallback, CoreSocket);
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
		closed = false;
		this.OnClientConnected?.Invoke();
		timerCheck?.Dispose();
		timerCheck = new Timer(TimerCheckServer, null, 2000, 5000);
		return OperateResult.CreateSuccessResult();
	}

	public void ConnectClose()
	{
		if (!closed)
		{
			closed = true;
			HslHelper.ThreadSleep(20);
			CoreSocket?.Close();
			timerCheck?.Dispose();
		}
	}

	public async Task<OperateResult> ConnectServerAsync()
	{
		CoreSocket?.Close();
		OperateResult<Socket> connect = await CreateSocketAndConnectAsync(ipAddress, port, connectTimeOut);
		if (!connect.IsSuccess)
		{
			return connect;
		}
		CoreSocket = connect.Content;
		try
		{
			CoreSocket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveAsyncCallback, CoreSocket);
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
		closed = false;
		this.OnClientConnected?.Invoke();
		timerCheck?.Dispose();
		timerCheck = new Timer(TimerCheckServer, null, 2000, 5000);
		return OperateResult.CreateSuccessResult();
	}

	private void OnVibrationSensorClientNetworkError()
	{
		if (closed || Interlocked.CompareExchange(ref isReConnectServer, 1, 0) != 0)
		{
			return;
		}
		try
		{
			if (this.OnNetworkError == null)
			{
				base.LogNet?.WriteInfo("The network is abnormal, and the system is ready to automatically reconnect after 10 seconds.");
				while (true)
				{
					for (int i = 0; i < 10; i++)
					{
						HslHelper.ThreadSleep(1000);
						base.LogNet?.WriteInfo($"Wait for {10 - i} second to connect to the server ...");
					}
					OperateResult operateResult = ConnectServer();
					if (operateResult.IsSuccess)
					{
						break;
					}
					base.LogNet?.WriteInfo("The connection failed. Prepare to reconnect after 10 seconds.");
				}
				base.LogNet?.WriteInfo("Successfully connected to the server!");
			}
			else
			{
				this.OnNetworkError?.Invoke(this, new EventArgs());
			}
			activeTime = DateTime.Now;
			Interlocked.Exchange(ref isReConnectServer, 0);
		}
		catch
		{
			Interlocked.Exchange(ref isReConnectServer, 0);
			throw;
		}
	}

	private async void ReceiveAsyncCallback(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (!(asyncState is Socket socket))
		{
			return;
		}
		try
		{
			socket.EndReceive(ar);
		}
		catch (ObjectDisposedException)
		{
			socket?.Close();
			base.LogNet?.WriteDebug(ToString(), "Closed");
			return;
		}
		catch (Exception ex2)
		{
			Exception ex4 = ex2;
			Exception ex5 = ex4;
			socket?.Close();
			base.LogNet?.WriteDebug(ToString(), "ReceiveCallback Failed:" + ex5.Message);
			OnVibrationSensorClientNetworkError();
			return;
		}
		if (closed)
		{
			base.LogNet?.WriteDebug(ToString(), "Closed");
			return;
		}
		OperateResult<byte[]> read = await ReceiveAsync(socket, 9);
		if (!read.IsSuccess)
		{
			OnVibrationSensorClientNetworkError();
			return;
		}
		if (read.Content[0] == 170 && read.Content[1] == 85 && read.Content[2] == 127 && read.Content[7] == 0)
		{
			OperateResult<byte[]> read3 = await ReceiveAsync(socket, 3);
			if (!read3.IsSuccess)
			{
				OnVibrationSensorClientNetworkError();
				return;
			}
			int length = read3.Content[1] * 256 + read3.Content[2];
			OperateResult<byte[]> read4 = await ReceiveAsync(socket, length + 4);
			if (!read4.IsSuccess)
			{
				OnVibrationSensorClientNetworkError();
				return;
			}
			if (read.Content[5] == 1)
			{
				Address = byteTransform.TransUInt16(read.Content, 3);
				base.LogNet?.WriteDebug("Receive: " + SoftBasic.SpliceArray<byte>(read.Content, read3.Content, read4.Content).ToHexString(' '));
				VibrationSensorPeekValue peekValue = new VibrationSensorPeekValue
				{
					AcceleratedSpeedX = (float)BitConverter.ToInt16(read4.Content, 0) / 100f,
					AcceleratedSpeedY = (float)BitConverter.ToInt16(read4.Content, 2) / 100f,
					AcceleratedSpeedZ = (float)BitConverter.ToInt16(read4.Content, 4) / 100f,
					SpeedX = (float)BitConverter.ToInt16(read4.Content, 6) / 100f,
					SpeedY = (float)BitConverter.ToInt16(read4.Content, 8) / 100f,
					SpeedZ = (float)BitConverter.ToInt16(read4.Content, 10) / 100f,
					OffsetX = BitConverter.ToInt16(read4.Content, 12),
					OffsetY = BitConverter.ToInt16(read4.Content, 14),
					OffsetZ = BitConverter.ToInt16(read4.Content, 16),
					Temperature = (float)BitConverter.ToInt16(read4.Content, 18) * 0.02f - 273.15f,
					Voltage = (float)BitConverter.ToInt16(read4.Content, 20) / 100f,
					SendingInterval = BitConverter.ToInt32(read4.Content, 22)
				};
				this.OnPeekValueReceive?.Invoke(peekValue);
			}
		}
		else if (read.Content[0] == 170)
		{
			VibrationSensorActualValue actualValue = new VibrationSensorActualValue
			{
				AcceleratedSpeedX = (float)byteTransform.TransInt16(read.Content, 1) / 100f,
				AcceleratedSpeedY = (float)byteTransform.TransInt16(read.Content, 3) / 100f,
				AcceleratedSpeedZ = (float)byteTransform.TransInt16(read.Content, 5) / 100f
			};
			this.OnActualValueReceive?.Invoke(actualValue);
		}
		else
		{
			OperateResult<byte[]> read5 = await ReceiveAsync(socket, 9);
			if (!read5.IsSuccess)
			{
				OnVibrationSensorClientNetworkError();
				return;
			}
			byte[] array = SoftBasic.SpliceArray<byte>(read.Content, read5.Content);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == 170)
				{
					if (i >= 9)
					{
						await ReceiveAsync(socket, i - 9);
						break;
					}
					if (array[i + 9] == 170)
					{
						await ReceiveAsync(socket, i);
						break;
					}
				}
			}
		}
		activeTime = DateTime.Now;
		try
		{
			socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, ReceiveAsyncCallback, socket);
		}
		catch (Exception ex6)
		{
			socket?.Close();
			base.LogNet?.WriteDebug(ToString(), "BeginReceive Failed:" + ex6.Message);
			OnVibrationSensorClientNetworkError();
		}
	}

	private void TimerCheckServer(object obj)
	{
		if (CoreSocket == null || closed)
		{
			return;
		}
		if ((DateTime.Now - activeTime).TotalSeconds > (double)checkSeconds)
		{
			if (CheckTimeoutCount == 0)
			{
				base.LogNet?.WriteDebug(StringResources.Language.NetHeartCheckTimeout);
			}
			CheckTimeoutCount = 1;
			OnVibrationSensorClientNetworkError();
		}
		else
		{
			CheckTimeoutCount = 0;
		}
	}

	private OperateResult SendPre(byte[] send)
	{
		base.LogNet?.WriteDebug("Send " + send.ToHexString(' '));
		return Send(CoreSocket, send);
	}

	[HslMqttApi]
	public OperateResult SetReadStatus()
	{
		return SendPre(BulidLongMessage(address, 1, null));
	}

	[HslMqttApi]
	public OperateResult SetReadActual()
	{
		return SendPre(BulidLongMessage(address, 2, null));
	}

	[HslMqttApi]
	public OperateResult SetReadStatusInterval(int seconds)
	{
		byte[] array = new byte[6]
		{
			BitConverter.GetBytes(address)[0],
			BitConverter.GetBytes(address)[1],
			0,
			0,
			0,
			0
		};
		BitConverter.GetBytes(seconds).CopyTo(array, 2);
		return SendPre(BulidLongMessage(address, 16, array));
	}

	public override string ToString()
	{
		return $"VibrationSensorClient[{ipAddress}:{port}]";
	}

	public static byte[] BulidLongMessage(ushort address, byte cmd, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		byte[] array = new byte[16 + data.Length];
		array[0] = 170;
		array[1] = 85;
		array[2] = 127;
		array[3] = BitConverter.GetBytes(address)[1];
		array[4] = BitConverter.GetBytes(address)[0];
		array[5] = cmd;
		array[6] = 1;
		array[7] = 0;
		array[8] = 1;
		array[9] = 1;
		array[10] = BitConverter.GetBytes(data.Length)[1];
		array[11] = BitConverter.GetBytes(data.Length)[0];
		data.CopyTo(array, 12);
		int num = array[3];
		for (int i = 4; i < array.Length - 4; i++)
		{
			num ^= array[i];
		}
		array[array.Length - 4] = (byte)num;
		array[array.Length - 3] = 127;
		array[array.Length - 2] = 170;
		array[array.Length - 1] = 237;
		return array;
	}

	public static bool CheckXor(byte[] data)
	{
		int num = data[3];
		for (int i = 4; i < data.Length - 4; i++)
		{
			num ^= data[i];
		}
		return BitConverter.GetBytes(num)[0] == data[data.Length - 4];
	}
}
