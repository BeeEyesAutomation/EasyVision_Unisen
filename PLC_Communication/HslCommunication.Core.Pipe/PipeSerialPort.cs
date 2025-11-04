using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Pipe;

public class PipeSerialPort : CommunicationPipe, IDisposable
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

	public bool DtrEnable
	{
		get
		{
			return serialPort.DtrEnable;
		}
		set
		{
			serialPort.DtrEnable = value;
		}
	}

	public int AtLeastReceiveLength { get; set; } = 1;

	[HslMqttApi(Description = "Get or set the number of consecutive empty data receptions, which is valid when data reception is completed, default is 1")]
	public int ReceiveEmptyDataCount { get; set; } = 1;

	[HslMqttApi(Description = "Whether to empty the buffer before sending data, the default is false")]
	public bool IsClearCacheBeforeRead { get; set; }

	public PipeSerialPort()
	{
		serialPort = new SerialPort();
		base.SleepTime = 20;
	}

	public PipeSerialPort(string portName)
	{
		serialPort = new SerialPort();
		base.SleepTime = 20;
		SerialPortInni(portName);
	}

	public void SerialPortInni(string portName)
	{
		if (portName.Contains("-") || portName.Contains(";"))
		{
			SerialPortInni(delegate(SerialPort sp)
			{
				sp.IniSerialByFormatString(portName);
			});
		}
		else
		{
			SerialPortInni(portName, 9600, 8, StopBits.One, Parity.None);
		}
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

	public bool IsOpen()
	{
		return serialPort.IsOpen;
	}

	public SerialPort GetPipe()
	{
		return serialPort;
	}

	public OperateResult<byte[]> ClearSerialCache()
	{
		return SPReceived(serialPort, null, null, awaitData: false);
	}

	public override OperateResult<bool> OpenCommunication()
	{
		try
		{
			if (!serialPort.IsOpen)
			{
				serialPort.Open();
				ResetConnectErrorCount();
				return OperateResult.CreateSuccessResult(value: true);
			}
			return OperateResult.CreateSuccessResult(value: false);
		}
		catch (Exception ex)
		{
			return new OperateResult<bool>("OpenCommunication failed: " + ex.Message);
		}
	}

	public override OperateResult CloseCommunication()
	{
		if (serialPort.IsOpen)
		{
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

	public override OperateResult Send(byte[] data, int offset, int size)
	{
		if (data != null && data.Length != 0)
		{
			if (!Authorization.nzugaydgwadawdibbas())
			{
				return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
			}
			try
			{
				serialPort.Write(data, offset, size);
				return OperateResult.CreateSuccessResult();
			}
			catch (Exception ex)
			{
				return new OperateResult(-IncrConnectErrorCount(), ex.Message);
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public override OperateResult<int> Receive(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		if (!Authorization.nzugaydgwadawdibbas())
		{
			return new OperateResult<int>(StringResources.Language.AuthorizationFailed);
		}
		try
		{
			if (length > 0)
			{
				int value = serialPort.Read(buffer, offset, length);
				return OperateResult.CreateSuccessResult(value);
			}
			int value2 = serialPort.Read(buffer, offset, buffer.Length - offset);
			return OperateResult.CreateSuccessResult(value2);
		}
		catch (Exception ex)
		{
			return new OperateResult<int>(-IncrConnectErrorCount(), ex.Message);
		}
	}

	private OperateResult<byte[]> SPReceived(SerialPort serialPort, INetMessage netMessage, byte[] sendValue, bool awaitData, Action<byte[]> logMessage = null)
	{
		if (!Authorization.nzugaydgwadawdibbas())
		{
			return new OperateResult<byte[]>(StringResources.Language.AuthorizationFailed);
		}
		byte[] array = null;
		MemoryStream memoryStream = null;
		try
		{
			array = new byte[1024];
			memoryStream = new MemoryStream();
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
		DateTime now = DateTime.Now;
		int num = 0;
		int num2 = 0;
		while (true)
		{
			num2++;
			if (num2 > 1 && base.SleepTime >= 0)
			{
				HslHelper.ThreadSleep(base.SleepTime);
			}
			try
			{
				if (serialPort.BytesToRead < 1)
				{
					if (num2 == 1)
					{
						continue;
					}
					if ((DateTime.Now - now).TotalMilliseconds > (double)base.ReceiveTimeOut)
					{
						return new OperateResult<byte[]>(-IncrConnectErrorCount(), $"Time out: {base.ReceiveTimeOut}, received: {memoryStream.ToArray().ToHexString(' ')}");
					}
					if (memoryStream.Length >= AtLeastReceiveLength)
					{
						num++;
						if (netMessage != null || num < ReceiveEmptyDataCount)
						{
							continue;
						}
					}
					else if (awaitData)
					{
						continue;
					}
					break;
				}
				num = 0;
				int num3 = serialPort.Read(array, 0, array.Length);
				if (num3 > 0)
				{
					memoryStream.Write(array, 0, num3);
					logMessage?.Invoke(array.SelectBegin(num3));
				}
				if (netMessage != null && CheckMessageComplete(netMessage, sendValue, ref memoryStream))
				{
					break;
				}
				if (base.ReceiveTimeOut > 0 && (DateTime.Now - now).TotalMilliseconds > (double)base.ReceiveTimeOut)
				{
					return new OperateResult<byte[]>(-IncrConnectErrorCount(), $"Time out: {base.ReceiveTimeOut}, received: {memoryStream.ToArray().ToHexString(' ')}");
				}
				continue;
			}
			catch (Exception ex2)
			{
				return new OperateResult<byte[]>(-IncrConnectErrorCount(), ex2.Message);
			}
		}
		ResetConnectErrorCount();
		return OperateResult.CreateSuccessResult(memoryStream.ToArray());
	}

	public override OperateResult<byte[]> ReceiveMessage(INetMessage netMessage, byte[] sendValue, bool useActivePush = true, Action<long, long> reportProgress = null, Action<byte[]> logMessage = null)
	{
		if (base.UseServerActivePush)
		{
			return base.ReceiveMessage(netMessage, sendValue, useActivePush, reportProgress);
		}
		return SPReceived(serialPort, netMessage, sendValue, awaitData: true, logMessage);
	}

	public override OperateResult<byte[]> ReadFromCoreServer(INetMessage netMessage, byte[] sendValue, bool hasResponseData, Action<byte[]> logMessage = null)
	{
		if (IsClearCacheBeforeRead)
		{
			ClearSerialCache();
		}
		OperateResult<byte[]> operateResult = ReadFromCoreServerHelper(netMessage, sendValue, hasResponseData, 0, logMessage);
		if (operateResult.IsSuccess)
		{
			ResetConnectErrorCount();
		}
		return operateResult;
	}

	public override async Task<OperateResult<byte[]>> ReceiveMessageAsync(INetMessage netMessage, byte[] sendValue, bool useActivePush = true, Action<long, long> reportProgress = null, Action<byte[]> logMessage = null)
	{
		return await Task.Run(() => SPReceived(serialPort, netMessage, sendValue, awaitData: true, logMessage)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(INetMessage netMessage, byte[] sendValue, bool hasResponseData, Action<byte[]> logMessage = null)
	{
		return await Task.Run(() => ReadFromCoreServer(netMessage, sendValue, hasResponseData, logMessage)).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		serialPort?.Dispose();
	}

	public override string ToString()
	{
		return "PipeSerialPort[" + serialPort.ToFormatString() + "]";
	}
}
