using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.Profinet.Beckhoff.Helper;

internal class AdsHelper
{
	public static byte[] BuildAmsHeaderCommand(ushort commandId, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		byte[] array = new byte[32 + data.Length];
		array[16] = BitConverter.GetBytes(commandId)[0];
		array[17] = BitConverter.GetBytes(commandId)[1];
		array[18] = 4;
		array[19] = 0;
		array[20] = BitConverter.GetBytes(data.Length)[0];
		array[21] = BitConverter.GetBytes(data.Length)[1];
		array[22] = BitConverter.GetBytes(data.Length)[2];
		array[23] = BitConverter.GetBytes(data.Length)[3];
		array[24] = 0;
		array[25] = 0;
		array[26] = 0;
		array[27] = 0;
		data.CopyTo(array, 32);
		return PackAmsTcpHelper(AmsTcpHeaderFlags.Command, array);
	}

	public static OperateResult<byte[]> BuildReadDeviceInfoCommand()
	{
		return OperateResult.CreateSuccessResult(BuildAmsHeaderCommand(1, null));
	}

	public static OperateResult<byte[]> BuildReadStateCommand()
	{
		return OperateResult.CreateSuccessResult(BuildAmsHeaderCommand(4, null));
	}

	public static OperateResult<byte[]> BuildWriteControlCommand(short state, short deviceState, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		byte[] array = new byte[8 + data.Length];
		return OperateResult.CreateSuccessResult(BuildAmsHeaderCommand(5, SoftBasic.SpliceArray<byte>(BitConverter.GetBytes(state), BitConverter.GetBytes(deviceState), BitConverter.GetBytes(data.Length), data)));
	}

	public static OperateResult<byte[]> BuildReadCommand(string address, int length, bool isBit)
	{
		OperateResult<uint, uint> operateResult = AnalysisAddress(address, isBit);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[12];
		BitConverter.GetBytes(operateResult.Content1).CopyTo(array, 0);
		BitConverter.GetBytes(operateResult.Content2).CopyTo(array, 4);
		BitConverter.GetBytes(length).CopyTo(array, 8);
		return OperateResult.CreateSuccessResult(BuildAmsHeaderCommand(2, array));
	}

	public static OperateResult<byte[]> BuildReadCommand(string[] address, ushort[] length)
	{
		byte[] array = new byte[12 * address.Length];
		int num = 0;
		for (int i = 0; i < address.Length; i++)
		{
			OperateResult<uint, uint> operateResult = AnalysisAddress(address[i], isBit: false);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			BitConverter.GetBytes(operateResult.Content1).CopyTo(array, 12 * i);
			BitConverter.GetBytes(operateResult.Content2).CopyTo(array, 12 * i + 4);
			BitConverter.GetBytes((int)length[i]).CopyTo(array, 12 * i + 8);
			num += length[i];
		}
		return BuildReadWriteCommand("ig=0xF080;0", num, isBit: false, array);
	}

	public static OperateResult<byte[]> BuildReadWriteCommand(string address, int length, bool isBit, byte[] value)
	{
		OperateResult<uint, uint> operateResult = AnalysisAddress(address, isBit);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[16 + value.Length];
		BitConverter.GetBytes(operateResult.Content1).CopyTo(array, 0);
		BitConverter.GetBytes(operateResult.Content2).CopyTo(array, 4);
		BitConverter.GetBytes(length).CopyTo(array, 8);
		BitConverter.GetBytes(value.Length).CopyTo(array, 12);
		value.CopyTo(array, 16);
		return OperateResult.CreateSuccessResult(BuildAmsHeaderCommand(9, array));
	}

	public static OperateResult<byte[]> BuildWriteCommand(string[] address, List<byte[]> value)
	{
		MemoryStream memoryStream = new MemoryStream();
		int num = 0;
		for (int i = 0; i < address.Length; i++)
		{
			OperateResult<uint, uint> operateResult = AnalysisAddress(address[i], isBit: false);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			memoryStream.Write(BitConverter.GetBytes(operateResult.Content1));
			memoryStream.Write(BitConverter.GetBytes(operateResult.Content2));
			memoryStream.Write(BitConverter.GetBytes(value[i].Length));
			memoryStream.Write(value[i]);
			num += value[i].Length;
		}
		return BuildReadWriteCommand("ig=0xF081;0", num, isBit: false, memoryStream.ToArray());
	}

	public static OperateResult<byte[]> BuildWriteCommand(string address, byte[] value, bool isBit)
	{
		OperateResult<uint, uint> operateResult = AnalysisAddress(address, isBit);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = new byte[12 + value.Length];
		BitConverter.GetBytes(operateResult.Content1).CopyTo(array, 0);
		BitConverter.GetBytes(operateResult.Content2).CopyTo(array, 4);
		BitConverter.GetBytes(value.Length).CopyTo(array, 8);
		value.CopyTo(array, 12);
		return OperateResult.CreateSuccessResult(BuildAmsHeaderCommand(3, array));
	}

	public static OperateResult<byte[]> BuildWriteCommand(string address, bool[] value, bool isBit)
	{
		OperateResult<uint, uint> operateResult = AnalysisAddress(address, isBit);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = value.Select((bool m) => (byte)(m ? 1u : 0u)).ToArray();
		byte[] array2 = new byte[12 + array.Length];
		BitConverter.GetBytes(operateResult.Content1).CopyTo(array2, 0);
		BitConverter.GetBytes(operateResult.Content2).CopyTo(array2, 4);
		BitConverter.GetBytes(array.Length).CopyTo(array2, 8);
		array.CopyTo(array2, 12);
		return OperateResult.CreateSuccessResult(BuildAmsHeaderCommand(3, array2));
	}

	public static OperateResult<byte[]> BuildReleaseSystemHandle(uint handle)
	{
		byte[] array = new byte[16];
		BitConverter.GetBytes(61446).CopyTo(array, 0);
		BitConverter.GetBytes(4).CopyTo(array, 8);
		BitConverter.GetBytes(handle).CopyTo(array, 12);
		return OperateResult.CreateSuccessResult(BuildAmsHeaderCommand(3, array));
	}

	public static OperateResult<int> CheckResponse(byte[] response)
	{
		try
		{
			int num = BitConverter.ToInt32(response, 30);
			if (num > 0)
			{
				return new OperateResult<int>(num, GetErrorCodeText(num) + Environment.NewLine + "Source:" + response.ToHexString(' '));
			}
			if (response.Length >= 42)
			{
				int num2 = BitConverter.ToInt32(response, 38);
				if (num2 != 0)
				{
					return new OperateResult<int>(num2, GetErrorCodeText(num2) + Environment.NewLine + "Source:" + response.ToHexString(' '));
				}
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<int>(ex.Message + " Source:" + response.ToHexString(' '));
		}
		return OperateResult.CreateSuccessResult(0);
	}

	public static byte[] PackAmsTcpHelper(AmsTcpHeaderFlags headerFlags, byte[] command)
	{
		byte[] array = new byte[6 + command.Length];
		BitConverter.GetBytes((ushort)headerFlags).CopyTo(array, 0);
		BitConverter.GetBytes(command.Length).CopyTo(array, 2);
		command.CopyTo(array, 6);
		return array;
	}

	private static int CalculateAddressStarted(string address)
	{
		if (address.IndexOf('.') < 0)
		{
			return Convert.ToInt32(address);
		}
		string[] array = address.Split('.');
		return Convert.ToInt32(array[0]) * 8 + HslHelper.CalculateBitStartIndex(array[1]);
	}

	public static OperateResult<uint, uint> AnalysisAddress(string address, bool isBit)
	{
		OperateResult<uint, uint> operateResult = new OperateResult<uint, uint>();
		try
		{
			if (address.StartsWith("i=") || address.StartsWith("I="))
			{
				operateResult.Content1 = 61445u;
				operateResult.Content2 = uint.Parse(address.Substring(2));
			}
			else if (address.StartsWith("s=") || address.StartsWith("S="))
			{
				operateResult.Content1 = 61443u;
				operateResult.Content2 = 0u;
			}
			else if (address.StartsWith("ig=") || address.StartsWith("IG="))
			{
				address = address.ToUpper();
				operateResult.Content1 = (uint)HslHelper.ExtractParameter(ref address, "IG", 0);
				operateResult.Content2 = uint.Parse(address);
			}
			else
			{
				switch (address[0])
				{
				case 'M':
				case 'm':
					if (isBit)
					{
						operateResult.Content1 = 16417u;
						operateResult.Content2 = (uint)CalculateAddressStarted(address.Substring(1));
					}
					else
					{
						operateResult.Content1 = 16416u;
						operateResult.Content2 = uint.Parse(address.Substring(1));
					}
					break;
				case 'I':
				case 'i':
					if (isBit)
					{
						operateResult.Content1 = 61473u;
						operateResult.Content2 = (uint)(CalculateAddressStarted(address.Substring(1)) + 1024000);
					}
					else
					{
						operateResult.Content1 = 61472u;
						operateResult.Content2 = uint.Parse(address.Substring(1)) + 128000;
					}
					break;
				case 'Q':
				case 'q':
					if (isBit)
					{
						operateResult.Content1 = 61489u;
						operateResult.Content2 = (uint)(CalculateAddressStarted(address.Substring(1)) + 2048000);
					}
					else
					{
						operateResult.Content1 = 61488u;
						operateResult.Content2 = uint.Parse(address.Substring(1)) + 256000;
					}
					break;
				default:
					throw new Exception(StringResources.Language.NotSupportedDataType);
				}
			}
		}
		catch (Exception ex)
		{
			operateResult.Message = ex.Message;
			return operateResult;
		}
		operateResult.IsSuccess = true;
		operateResult.Message = StringResources.Language.SuccessText;
		return operateResult;
	}

	public static byte[] StrToAdsBytes(string value)
	{
		return SoftBasic.SpliceArray<byte>(Encoding.ASCII.GetBytes(value), new byte[1]);
	}

	public static byte[] StrToAMSNetId(string amsNetId)
	{
		string text = amsNetId;
		byte[] array;
		if (amsNetId.IndexOf(':') > 0)
		{
			array = new byte[8];
			string[] array2 = amsNetId.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			text = array2[0];
			array[6] = BitConverter.GetBytes(int.Parse(array2[1]))[0];
			array[7] = BitConverter.GetBytes(int.Parse(array2[1]))[1];
		}
		else
		{
			array = new byte[6];
		}
		string[] array3 = text.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array3.Length; i++)
		{
			array[i] = byte.Parse(array3[i]);
		}
		return array;
	}

	public static string GetAmsNetIdString(byte[] data, int index)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(data[index]);
		stringBuilder.Append(".");
		stringBuilder.Append(data[index + 1]);
		stringBuilder.Append(".");
		stringBuilder.Append(data[index + 2]);
		stringBuilder.Append(".");
		stringBuilder.Append(data[index + 3]);
		stringBuilder.Append(".");
		stringBuilder.Append(data[index + 4]);
		stringBuilder.Append(".");
		stringBuilder.Append(data[index + 5]);
		stringBuilder.Append(":");
		stringBuilder.Append(BitConverter.ToUInt16(data, index + 6));
		return stringBuilder.ToString();
	}

	public static string GetErrorCodeText(int error)
	{
		if (1 == 0)
		{
		}
		string result = error switch
		{
			0 => "NO ERROR", 
			1 => "InternalError", 
			2 => "NO RTIME", 
			3 => "Allocation locked – memory error.", 
			4 => "Mailbox full – the ADS message could not be sent. Reducing the number of ADS messages per cycle will help.", 
			5 => "WRONG RECEIVEH MSG", 
			6 => "Target port not found – ADS server is not started or is not reachable.", 
			7 => "Target computer not found – AMS route was not found.", 
			8 => "Unknown command ID.", 
			9 => "Invalid task ID.", 
			10 => "No IO.", 
			11 => "Unknown AMS command.", 
			12 => "Win32 error.", 
			13 => "Port not connected.", 
			14 => "Invalid AMS length.", 
			15 => "Invalid AMS Net ID.", 
			16 => "Installation level is too low –TwinCAT 2 license error.", 
			17 => "No debugging available.", 
			18 => "Port disabled – TwinCAT system service not started.", 
			19 => "Port already connected.", 
			20 => "AMS Sync Win32 error.", 
			21 => "AMS Sync Timeout.", 
			22 => "AMS Sync error.", 
			23 => "No index map for AMS Sync available.", 
			24 => "Invalid AMS port.", 
			25 => "No memory.", 
			26 => "TCP send error.", 
			27 => "Host unreachable.", 
			28 => "Invalid AMS fragment.", 
			29 => "TLS send error – secure ADS connection failed.", 
			30 => "Access denied – secure ADS access denied.", 
			1280 => "Locked memory cannot be allocated.", 
			1281 => "The router memory size could not be changed.", 
			1282 => "The mailbox has reached the maximum number of possible messages.", 
			1283 => "The Debug mailbox has reached the maximum number of possible messages.", 
			1284 => "The port type is unknown.", 
			1285 => "The router is not initialized.", 
			1286 => "The port number is already assigned.", 
			1287 => "The port is not registered.", 
			1288 => "The maximum number of ports has been reached.", 
			1289 => "The port is invalid.", 
			1290 => "The router is not active.", 
			1291 => "The mailbox has reached the maximum number for fragmented messages.", 
			1292 => "A fragment timeout has occurred.", 
			1293 => "The port is removed.", 
			1792 => "General device error.", 
			1793 => "Service is not supported by the server.", 
			1794 => "Invalid index group.", 
			1795 => "Invalid index offset.", 
			1796 => "Reading or writing not permitted.", 
			1797 => "Parameter size not correct. Commonly found in batch processing, check the calculation command length", 
			1798 => "Invalid data values.", 
			1799 => "Device is not ready to operate. It is possible that the TSM configuration is incorrect, reactivate the configuration", 
			1800 => "Device Busy", 
			1801 => "Invalid operating system context. This can result from use of ADS blocks in different tasks. It may be possible to resolve this through multitasking synchronization in the PLC.", 
			1802 => "Insufficient memory.", 
			1803 => "Invalid parameter values.", 
			1804 => "Device Not Found", 
			1805 => "Device Syntax Error", 
			1806 => "Objects do not match.", 
			1807 => "Object already exists.", 
			1808 => "Symbol not found. Check whether the variable name is correct, Note: the global variables in some PLC equipment are: .[Variable Name]", 
			1809 => "Invalid symbol version. This can occur due to an online change. Create a new handle.", 
			1810 => "Device (server) is in invalid state.", 
			1811 => "AdsTransMode not supported.", 
			1812 => "Device Notify Handle Invalid", 
			1813 => "Notification client not registered.", 
			1814 => "Device No More Handles", 
			1815 => "Device Invalid Watch size", 
			1816 => "Device Not Initialized", 
			1817 => "Device TimeOut", 
			1818 => "Device No Interface", 
			1819 => "Device Invalid Interface", 
			1820 => "Device Invalid CLSID", 
			1821 => "Device Invalid Object ID", 
			1822 => "Device Request Is Pending", 
			1823 => "Device Request Is Aborted", 
			1824 => "Device Signal Warning", 
			1825 => "Device Invalid Array Index", 
			1826 => "Device Symbol Not Active", 
			1827 => "Device Access Denied", 
			1828 => "Device Missing License", 
			1829 => "Device License Expired", 
			1830 => "Device License Exceeded", 
			1831 => "Device License Invalid", 
			1832 => "Device License System Id", 
			1833 => "Device License No Time Limit", 
			1834 => "Device License Future Issue", 
			1835 => "Device License Time To Long", 
			1836 => "Device Exception During Startup", 
			1837 => "Device License Duplicated", 
			1838 => "Device Signature Invalid", 
			1839 => "Device Certificate Invalid", 
			1840 => "Device License Oem Not Found", 
			1841 => "Device License Restricted", 
			1842 => "Device License Demo Denied", 
			1843 => "Device Invalid Function Id", 
			1844 => "Device Out Of Range", 
			1845 => "Device Invalid Alignment", 
			1846 => "Device License Platform", 
			1847 => "Device Context Forward Passive Level", 
			1848 => "Device Context Forward Dispatch Level", 
			1849 => "Device Context Forward RealTime", 
			1850 => "Device Certificate Entrust", 
			1856 => "ClientError", 
			1857 => "Client Invalid Parameter", 
			1858 => "Client List Empty", 
			1859 => "Client Variable In Use", 
			1860 => "Client Duplicate InvokeID", 
			1861 => "Timeout has occurred – the remote terminal is not responding in the specified ADS timeout. The route setting of the remote terminal may be configured incorrectly.", 
			1862 => "ClientW32OR", 
			1863 => "Client Timeout Invalid", 
			1864 => "Client Port Not Open", 
			1865 => "Client No Ams Addr", 
			1872 => "Client Sync Internal", 
			1873 => "Client Add Hash", 
			1874 => "Client Remove Hash", 
			1875 => "Client No More Symbols", 
			1876 => "Client Response Invalid", 
			1877 => "Client Port Locked", 
			32768 => "ClientQueueFull", 
			10060 => "A connection timeout has occurred - error while establishing the connection, because the remote terminal did not respond properly after a certain period of time", 
			10061 => "WSA_ConnRefused", 
			10065 => "No route to host - a socket operation referred to an unavailable host.", 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
