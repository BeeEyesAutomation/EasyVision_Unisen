using System;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Pipe;

public class PipeMoxa : CommunicationPipe, IDisposable
{
	private int portNumber = -1;

	private bool rts = false;

	private bool dtr = false;

	private bool isOpen = false;

	private int moxaBaudRate = 12;

	private int moxaDataBits = 3;

	private int moxaStopBits = 0;

	private int moxaParity = 0;

	private string moxaFormate = string.Empty;

	public bool RtsEnable
	{
		get
		{
			return rts;
		}
		set
		{
			rts = value;
			if (isOpen)
			{
				PCommHelper.sio_RTS(portNumber, value ? 1 : 0);
			}
		}
	}

	public bool DtrEnable
	{
		get
		{
			return dtr;
		}
		set
		{
			dtr = value;
			if (isOpen)
			{
				PCommHelper.sio_DTR(portNumber, value ? 1 : 0);
			}
		}
	}

	public int AtLeastReceiveLength { get; set; } = 1;

	[HslMqttApi(Description = "Get or set the number of consecutive empty data receptions, which is valid when data reception is completed, default is 1")]
	public int ReceiveEmptyDataCount { get; set; } = 1;

	[HslMqttApi(Description = "Whether to empty the buffer before sending data, the default is false")]
	public bool IsClearCacheBeforeRead { get; set; }

	public PipeMoxa()
	{
		base.SleepTime = 5;
		ReceiveEmptyDataCount = 4;
	}

	public PipeMoxa(string portName)
	{
		base.SleepTime = 5;
		ReceiveEmptyDataCount = 4;
		SerialPortInni(portName);
	}

	public void SerialPortInni(string portName)
	{
		if (portName.Contains("-"))
		{
			string portName2 = "COM1";
			int baudRate = 9600;
			int dataBits = 8;
			Parity parity = Parity.None;
			StopBits stopBits = StopBits.One;
			string[] array = portName.Split(new char[2] { '-', ';' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 0)
			{
				return;
			}
			int num = 0;
			if (!Regex.IsMatch(array[0], "^[0-9]+$"))
			{
				portName2 = array[0];
				num = 1;
			}
			if (num < array.Length)
			{
				baudRate = Convert.ToInt32(array[num++]);
			}
			if (num < array.Length)
			{
				dataBits = Convert.ToInt32(array[num++]);
			}
			if (num < array.Length)
			{
				string text = array[num++].ToUpper();
				if (1 == 0)
				{
				}
				Parity parity2 = text switch
				{
					"E" => Parity.Even, 
					"O" => Parity.Odd, 
					"N" => Parity.None, 
					_ => Parity.Space, 
				};
				if (1 == 0)
				{
				}
				parity = parity2;
			}
			if (num < array.Length)
			{
				string text2 = array[num++];
				if (1 == 0)
				{
				}
				StopBits stopBits2 = text2 switch
				{
					"0" => StopBits.None, 
					"2" => StopBits.Two, 
					"1" => StopBits.One, 
					_ => StopBits.OnePointFive, 
				};
				if (1 == 0)
				{
				}
				stopBits = stopBits2;
			}
			SerialPortInni(portName2, baudRate, dataBits, stopBits, parity);
		}
		else
		{
			SerialPortInni(portName, 9600, 8, StopBits.One, Parity.None);
		}
	}

	public void SerialPortInni(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity)
	{
		if (portName.StartsWith("COM", StringComparison.OrdinalIgnoreCase))
		{
			portNumber = Convert.ToInt32(portName.Substring(3));
		}
		else
		{
			portNumber = Convert.ToInt32(portName);
		}
		switch (baudRate)
		{
		case 50:
			moxaBaudRate = 0;
			break;
		case 75:
			moxaBaudRate = 1;
			break;
		case 110:
			moxaBaudRate = 2;
			break;
		case 134:
			moxaBaudRate = 3;
			break;
		case 150:
			moxaBaudRate = 4;
			break;
		case 300:
			moxaBaudRate = 5;
			break;
		case 600:
			moxaBaudRate = 6;
			break;
		case 1200:
			moxaBaudRate = 7;
			break;
		case 1800:
			moxaBaudRate = 8;
			break;
		case 2400:
			moxaBaudRate = 9;
			break;
		case 4800:
			moxaBaudRate = 10;
			break;
		case 7200:
			moxaBaudRate = 11;
			break;
		case 9600:
			moxaBaudRate = 12;
			break;
		case 19200:
			moxaBaudRate = 13;
			break;
		case 38400:
			moxaBaudRate = 14;
			break;
		case 57600:
			moxaBaudRate = 15;
			break;
		case 115200:
			moxaBaudRate = 16;
			break;
		case 230400:
			moxaBaudRate = 17;
			break;
		case 460800:
			moxaBaudRate = 18;
			break;
		case 921600:
			moxaBaudRate = 19;
			break;
		}
		switch (dataBits)
		{
		case 5:
			moxaDataBits = 0;
			break;
		case 6:
			moxaDataBits = 1;
			break;
		case 7:
			moxaDataBits = 2;
			break;
		case 8:
			moxaDataBits = 3;
			break;
		}
		switch (stopBits)
		{
		case StopBits.One:
			moxaStopBits = 0;
			break;
		case StopBits.Two:
			moxaStopBits = 4;
			break;
		}
		switch (parity)
		{
		case Parity.None:
			moxaParity = 0;
			break;
		case Parity.Even:
			moxaParity = 24;
			break;
		case Parity.Odd:
			moxaParity = 8;
			break;
		case Parity.Mark:
			moxaParity = 40;
			break;
		case Parity.Space:
			moxaParity = 48;
			break;
		}
		moxaFormate = HslHelper.ToFormatString(portName, baudRate, dataBits, parity, stopBits);
	}

	public bool IsOpen()
	{
		return isOpen;
	}

	public OperateResult<byte[]> ClearSerialCache()
	{
		return SPReceived(null, null, awaitData: false);
	}

	private string GetErrorText(int ret)
	{
		if (1 == 0)
		{
		}
		string result = ret switch
		{
			0 => "OK", 
			-1 => "Port number is invalid.", 
			-2 => "The board is not the MOXA compatible intelligent board", 
			-4 => "No data to read.", 
			-5 => "No such port or port is occupied by other program.", 
			-6 => "Can't control the port because it is set as auto H/W flow control by sio_flowctrl.", 
			-7 => "Bad parameter.", 
			-8 => "Calling Win32 function failed Call GetLastError to get the error code.", 
			-9 => "The com port does not support this function.", 
			-11 => "User abort blocked write.", 
			-12 => "Write timeout.", 
			_ => "Unkown", 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public override OperateResult<bool> OpenCommunication()
	{
		try
		{
			if (!isOpen)
			{
				int num = PCommHelper.sio_open(portNumber);
				if (num == 0)
				{
					int mode = moxaDataBits | moxaParity | moxaStopBits;
					PCommHelper.sio_ioctl(portNumber, moxaBaudRate, mode);
					if (RtsEnable)
					{
						PCommHelper.sio_RTS(portNumber, 1);
					}
					if (DtrEnable)
					{
						PCommHelper.sio_DTR(portNumber, 1);
					}
					ResetConnectErrorCount();
					isOpen = true;
					return OperateResult.CreateSuccessResult(value: true);
				}
				return new OperateResult<bool>(num, GetErrorText(num));
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
		if (isOpen)
		{
			try
			{
				PCommHelper.sio_close(portNumber);
				isOpen = false;
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
				int num = 0;
				if (offset == 0)
				{
					num = PCommHelper.sio_write(portNumber, data, data.Length);
				}
				else
				{
					byte[] array = data.SelectMiddle(offset, size);
					num = PCommHelper.sio_write(portNumber, array, array.Length);
				}
				if (num >= 0)
				{
					return OperateResult.CreateSuccessResult();
				}
				return new OperateResult(num, GetErrorText(num));
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
				int num = PCommHelper.sio_read(portNumber, ref buffer[offset], length);
				if (num >= 0)
				{
					return OperateResult.CreateSuccessResult(num);
				}
				return new OperateResult<int>(num, GetErrorText(num));
			}
			int num2 = PCommHelper.sio_read(portNumber, ref buffer[offset], buffer.Length - offset);
			if (num2 >= 0)
			{
				return OperateResult.CreateSuccessResult(num2);
			}
			return new OperateResult<int>(num2, GetErrorText(num2));
		}
		catch (Exception ex)
		{
			return new OperateResult<int>(-IncrConnectErrorCount(), ex.Message);
		}
	}

	private OperateResult<byte[]> SPReceived(INetMessage netMessage, byte[] sendValue, bool awaitData, Action<byte[]> logMessage = null)
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
				if (PCommHelper.sio_iqueue(portNumber) < 1)
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
				int num3 = PCommHelper.sio_read(portNumber, ref array[0], array.Length);
				if (num3 < 0)
				{
					return new OperateResult<byte[]>(num3, GetErrorText(num3));
				}
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
		return SPReceived(netMessage, sendValue, awaitData: true, logMessage);
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
		return await Task.Run(() => SPReceived(netMessage, sendValue, awaitData: true, logMessage)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(INetMessage netMessage, byte[] sendValue, bool hasResponseData, Action<byte[]> logMessage = null)
	{
		return await Task.Run(() => ReadFromCoreServer(netMessage, sendValue, hasResponseData, logMessage)).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		CloseCommunication();
	}

	public string ToFormatString()
	{
		return moxaFormate;
	}

	public override string ToString()
	{
		return $"PipeMoxa[COM{portNumber}]";
	}
}
