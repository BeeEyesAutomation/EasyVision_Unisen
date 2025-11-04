using System;
using System.IO.Ports;

namespace HslCommunication.Core.Pipe;

[Obsolete("Use class PipeSerialPort instead")]
public class PipeSerial : PipeBase, IDisposable
{
	private SerialPort serialPort;

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

	public PipeSerial()
	{
		serialPort = new SerialPort();
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
		}
	}

	public void SerialPortInni(Action<SerialPort> initi)
	{
		if (!serialPort.IsOpen)
		{
			serialPort.PortName = "COM1";
			initi(serialPort);
		}
	}

	public OperateResult Open()
	{
		try
		{
			if (!serialPort.IsOpen)
			{
				serialPort.Open();
			}
			return OperateResult.CreateSuccessResult();
		}
		catch (Exception ex)
		{
			return new OperateResult(ex.Message);
		}
	}

	public bool IsOpen()
	{
		return serialPort.IsOpen;
	}

	public OperateResult Close(Func<SerialPort, OperateResult> extraOnClose)
	{
		if (serialPort.IsOpen)
		{
			if (extraOnClose != null)
			{
				OperateResult operateResult = extraOnClose(serialPort);
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
			}
			try
			{
				serialPort.Close();
			}
			catch (Exception ex)
			{
				return new OperateResult(ex.Message);
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public override void Dispose()
	{
		base.Dispose();
		serialPort?.Dispose();
	}

	public SerialPort GetPipe()
	{
		return serialPort;
	}

	public override string ToString()
	{
		return $"PipeSerial[{serialPort.PortName},{serialPort.BaudRate},{serialPort.DataBits},{serialPort.StopBits},{serialPort.Parity}]";
	}
}
