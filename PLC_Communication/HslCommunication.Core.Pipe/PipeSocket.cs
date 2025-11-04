using System;
using System.Net;
using System.Net.Sockets;

namespace HslCommunication.Core.Pipe;

public class PipeSocket : PipeBase, IDisposable
{
	private string ipAddress = "127.0.0.1";

	private int[] _port = new int[1] { 2000 };

	private int indexPort = -1;

	private Socket socket;

	private int receiveTimeOut = 5000;

	private int connectTimeOut = 10000;

	private int sleepTime = 0;

	public IPEndPoint LocalBinding { get; set; }

	public string IpAddress
	{
		get
		{
			return ipAddress;
		}
		set
		{
			ipAddress = HslHelper.GetIpAddressFromInput(value);
		}
	}

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

	public bool IsSocketError { get; set; }

	public Socket Socket
	{
		get
		{
			return socket;
		}
		set
		{
			socket = value;
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

	public PipeSocket()
	{
	}

	public PipeSocket(string ipAddress, int port)
	{
		this.ipAddress = ipAddress;
		_port = new int[1] { port };
	}

	public bool IsConnectitonError()
	{
		return IsSocketError || socket == null;
	}

	public override void Dispose()
	{
		base.Dispose();
		socket?.Close();
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

	public void ChangePorts()
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

	public override string ToString()
	{
		return $"PipeSocket[{ipAddress}:{Port}]";
	}
}
