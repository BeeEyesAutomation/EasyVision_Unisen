using System;
using System.IO.Ports;
using HslCommunication.Core.Pipe;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Device;

public class DeviceSerialPort : DeviceCommunication
{
	private PipeSerialPort pipe;

	public override CommunicationPipe CommunicationPipe
	{
		get
		{
			return base.CommunicationPipe;
		}
		set
		{
			base.CommunicationPipe = value;
			if (value is PipeSerialPort pipeSerialPort)
			{
				pipe = pipeSerialPort;
				PortName = pipe.GetPipe().PortName;
				BaudRate = pipe.GetPipe().BaudRate;
			}
		}
	}

	[HslMqttApi(Description = "Gets or sets a value indicating whether the request sending (RTS) signal is enabled in serial communication.")]
	public bool RtsEnable
	{
		get
		{
			return pipe.RtsEnable;
		}
		set
		{
			pipe.RtsEnable = value;
		}
	}

	[HslMqttApi(Description = "Get or set the number of consecutive empty data receptions, which is valid when data reception is completed, default is 1")]
	public int ReceiveEmptyDataCount
	{
		get
		{
			return pipe.ReceiveEmptyDataCount;
		}
		set
		{
			pipe.ReceiveEmptyDataCount = value;
		}
	}

	[HslMqttApi(Description = "Whether to empty the buffer before sending data, the default is false")]
	public bool IsClearCacheBeforeRead
	{
		get
		{
			return pipe.IsClearCacheBeforeRead;
		}
		set
		{
			pipe.IsClearCacheBeforeRead = value;
		}
	}

	[HslMqttApi(Description = "The port name of the current connection serial port information")]
	public string PortName { get; private set; }

	[HslMqttApi(Description = "Baud rate of current connection serial port information")]
	public int BaudRate { get; private set; }

	public DeviceSerialPort()
	{
		pipe = new PipeSerialPort();
		CommunicationPipe = pipe;
	}

	public virtual void SerialPortInni(string portName)
	{
		pipe.SerialPortInni(portName);
	}

	public virtual void SerialPortInni(string portName, int baudRate)
	{
		SerialPortInni(portName, baudRate, 8, StopBits.One, Parity.None);
	}

	public virtual void SerialPortInni(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity)
	{
		pipe.SerialPortInni(portName, baudRate, dataBits, stopBits, parity);
		PortName = portName;
		BaudRate = baudRate;
	}

	public void SerialPortInni(Action<SerialPort> initi)
	{
		pipe.SerialPortInni(initi);
		PortName = pipe.GetPipe().PortName;
		BaudRate = pipe.GetPipe().BaudRate;
	}

	public virtual OperateResult Open()
	{
		OperateResult<bool> operateResult = CommunicationPipe.OpenCommunication();
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content)
		{
			return InitializationOnConnect();
		}
		return OperateResult.CreateSuccessResult();
	}

	public bool IsOpen()
	{
		if (CommunicationPipe is PipeSerialPort pipeSerialPort)
		{
			return pipeSerialPort.GetPipe().IsOpen;
		}
		if (CommunicationPipe is PipeMoxa pipeMoxa)
		{
			return pipeMoxa.IsOpen();
		}
		return pipe.GetPipe().IsOpen;
	}

	public void Close()
	{
		if (CommunicationPipe is PipeSerialPort)
		{
			if (pipe.GetPipe().IsOpen)
			{
				ExtraOnDisconnect();
				pipe.CloseCommunication();
			}
		}
		else
		{
			ExtraOnDisconnect();
			CommunicationPipe.CloseCommunication();
		}
	}

	public override string ToString()
	{
		return $"DeviceSerialPort<{base.ByteTransform}>{{{CommunicationPipe}}}";
	}
}
