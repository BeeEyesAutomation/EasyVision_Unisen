using System;
using System.Collections.Generic;
using System.IO.Ports;
using HslCommunication.Core;
using HslCommunication.LogNet;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Toledo;

public class ToledoSerial
{
	public delegate void ToledoStandardDataReceivedDelegate(object sender, ToledoStandardData toledoStandardData);

	private SerialPort serialPort;

	private ILogNet logNet;

	private int receiveTimeout = 5000;

	public ILogNet LogNet
	{
		get
		{
			return logNet;
		}
		set
		{
			logNet = value;
		}
	}

	public bool RtsEnable
	{
		get
		{
			return serialPort.RtsEnable;
		}
		set
		{
			serialPort.RtsEnable = value;
		}
	}

	public string PortName { get; private set; }

	public int BaudRate { get; private set; }

	[HslMqttApi(Description = "Timeout for receiving data, default is 5000ms")]
	public int ReceiveTimeout
	{
		get
		{
			return receiveTimeout;
		}
		set
		{
			receiveTimeout = value;
		}
	}

	public bool HasChk { get; set; } = false;

	public event ToledoStandardDataReceivedDelegate OnToledoStandardDataReceived;

	public ToledoSerial()
	{
		serialPort = new SerialPort();
		serialPort.RtsEnable = true;
		serialPort.DataReceived += SerialPort_DataReceived;
	}

	private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		List<byte> list = new List<byte>();
		byte[] array = new byte[1024];
		int num = 0;
		do
		{
			IL_013f:
			HslHelper.ThreadSleep(20);
			if (serialPort.BytesToRead < 1)
			{
				num++;
				continue;
			}
			num = 0;
			try
			{
				int num2 = serialPort.Read(array, 0, Math.Min(serialPort.BytesToRead, array.Length));
				byte[] array2 = new byte[num2];
				Array.Copy(array, 0, array2, 0, num2);
				list.AddRange(array2);
				if (HasChk)
				{
					if (list.Count <= 15 || list[list.Count - 2] != 13)
					{
						goto IL_013f;
					}
				}
				else if ((list.Count <= 15 || list[list.Count - 1] != 13) && (list.Count <= 15 || list[list.Count - 2] != 13))
				{
					goto IL_013f;
				}
			}
			catch (Exception ex)
			{
				logNet?.WriteException(ToString(), "SerialPort_DataReceived", ex);
				return;
			}
			break;
		}
		while (num < 3);
		if (list.Count != 0)
		{
			byte[] array3 = list.ToArray();
			LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + array3.ToHexString(' '));
			ToledoStandardData toledoStandardData = null;
			try
			{
				toledoStandardData = new ToledoStandardData(array3);
			}
			catch (Exception ex2)
			{
				logNet?.WriteException(ToString(), "ToledoStandardData new failed: " + array3.ToHexString(' '), ex2);
			}
			if (toledoStandardData != null)
			{
				this.OnToledoStandardDataReceived?.Invoke(this, toledoStandardData);
			}
		}
	}

	public void SerialPortInni(string portName)
	{
		SerialPortInni(portName, 9600);
	}

	public void SerialPortInni(string portName, int baudRate)
	{
		SerialPortInni(portName, baudRate, 8, StopBits.One, Parity.None);
	}

	public void SerialPortInni(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity)
	{
		if (!serialPort.IsOpen)
		{
			serialPort.PortName = portName;
			serialPort.BaudRate = baudRate;
			serialPort.DataBits = dataBits;
			serialPort.StopBits = stopBits;
			serialPort.Parity = parity;
			PortName = serialPort.PortName;
			BaudRate = serialPort.BaudRate;
		}
	}

	public void SerialPortInni(Action<SerialPort> initi)
	{
		if (!serialPort.IsOpen)
		{
			serialPort.PortName = "COM5";
			serialPort.BaudRate = 9600;
			serialPort.DataBits = 8;
			serialPort.StopBits = StopBits.One;
			serialPort.Parity = Parity.None;
			initi(serialPort);
			PortName = serialPort.PortName;
			BaudRate = serialPort.BaudRate;
		}
	}

	public void Open()
	{
		if (!serialPort.IsOpen)
		{
			serialPort.Open();
		}
	}

	public bool IsOpen()
	{
		return serialPort.IsOpen;
	}

	public void Close()
	{
		if (serialPort.IsOpen)
		{
			serialPort.Close();
		}
	}

	public override string ToString()
	{
		return base.ToString();
	}
}
