using System;
using System.IO;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;

namespace HslCommunication.Profinet.Siemens.Helper;

public class SiemensPPIHelper
{
	public static OperateResult<S7AddressData> AnalysisAddress(string address)
	{
		S7AddressData s7AddressData = new S7AddressData();
		try
		{
			s7AddressData.DbBlock = 0;
			if (address.StartsWith("SYS"))
			{
				s7AddressData.DataCode = 3;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(3));
			}
			else if (address.StartsWith("AI"))
			{
				s7AddressData.DataCode = 6;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2));
			}
			else if (address.StartsWith("AQ"))
			{
				s7AddressData.DataCode = 7;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2));
			}
			else if (address[0] == 'T')
			{
				s7AddressData.DataCode = 31;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1));
			}
			else if (address[0] == 'C')
			{
				s7AddressData.DataCode = 30;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1));
			}
			else if (address.StartsWith("SM"))
			{
				s7AddressData.DataCode = 5;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(2));
			}
			else if (address[0] == 'S')
			{
				s7AddressData.DataCode = 4;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1));
			}
			else if (address[0] == 'I')
			{
				s7AddressData.DataCode = 129;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1));
			}
			else if (address[0] == 'Q')
			{
				s7AddressData.DataCode = 130;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1));
			}
			else if (address[0] == 'M')
			{
				s7AddressData.DataCode = 131;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1));
			}
			else if (address[0] == 'D' || address.StartsWith("DB"))
			{
				s7AddressData.DataCode = 132;
				string[] array = address.Split('.');
				if (address[1] == 'B')
				{
					s7AddressData.DbBlock = Convert.ToUInt16(array[0].Substring(2));
				}
				else
				{
					s7AddressData.DbBlock = Convert.ToUInt16(array[0].Substring(1));
				}
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(address.IndexOf('.') + 1));
			}
			else
			{
				if (address[0] != 'V')
				{
					return new OperateResult<S7AddressData>(StringResources.Language.NotSupportedDataType);
				}
				s7AddressData.DataCode = 132;
				s7AddressData.DbBlock = 1;
				s7AddressData.AddressStart = S7AddressData.CalculateAddressStarted(address.Substring(1));
			}
		}
		catch (Exception ex)
		{
			return new OperateResult<S7AddressData>(ex.Message);
		}
		return OperateResult.CreateSuccessResult(s7AddressData);
	}

	public static OperateResult<byte[]> BuildReadCommand(byte station, string address, ushort length, bool isBit)
	{
		OperateResult<S7AddressData> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return BuildReadCommand(station, operateResult.Content, length, isBit);
	}

	public static OperateResult<byte[]> BuildReadCommand(byte station, S7AddressData address, ushort length, bool isBit)
	{
		byte[] array = new byte[33];
		array[0] = 104;
		array[1] = BitConverter.GetBytes(array.Length - 6)[0];
		array[2] = BitConverter.GetBytes(array.Length - 6)[0];
		array[3] = 104;
		array[4] = station;
		array[5] = 0;
		array[6] = 108;
		array[7] = 50;
		array[8] = 1;
		array[9] = 0;
		array[10] = 0;
		array[11] = 0;
		array[12] = 0;
		array[13] = 0;
		array[14] = 14;
		array[15] = 0;
		array[16] = 0;
		array[17] = 4;
		array[18] = 1;
		array[19] = 18;
		array[20] = 10;
		array[21] = 16;
		array[22] = (byte)(isBit ? 1u : 2u);
		array[23] = 0;
		array[24] = BitConverter.GetBytes(length)[0];
		array[25] = BitConverter.GetBytes(length)[1];
		array[26] = (byte)address.DbBlock;
		array[27] = address.DataCode;
		array[28] = BitConverter.GetBytes(address.AddressStart)[2];
		array[29] = BitConverter.GetBytes(address.AddressStart)[1];
		array[30] = BitConverter.GetBytes(address.AddressStart)[0];
		int num = 0;
		for (int i = 4; i < 31; i++)
		{
			num += array[i];
		}
		array[31] = BitConverter.GetBytes(num)[0];
		array[32] = 22;
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteCommand(byte station, string address, byte[] values)
	{
		OperateResult<S7AddressData> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		int num = values.Length;
		byte[] array = new byte[37 + values.Length];
		array[0] = 104;
		array[1] = BitConverter.GetBytes(array.Length - 6)[0];
		array[2] = BitConverter.GetBytes(array.Length - 6)[0];
		array[3] = 104;
		array[4] = station;
		array[5] = 0;
		array[6] = 124;
		array[7] = 50;
		array[8] = 1;
		array[9] = 0;
		array[10] = 0;
		array[11] = 0;
		array[12] = 0;
		array[13] = 0;
		array[14] = 14;
		array[15] = 0;
		array[16] = (byte)(values.Length + 4);
		array[17] = 5;
		array[18] = 1;
		array[19] = 18;
		array[20] = 10;
		array[21] = 16;
		array[22] = 2;
		array[23] = 0;
		array[24] = BitConverter.GetBytes(num)[0];
		array[25] = BitConverter.GetBytes(num)[1];
		array[26] = (byte)operateResult.Content.DbBlock;
		array[27] = operateResult.Content.DataCode;
		array[28] = BitConverter.GetBytes(operateResult.Content.AddressStart)[2];
		array[29] = BitConverter.GetBytes(operateResult.Content.AddressStart)[1];
		array[30] = BitConverter.GetBytes(operateResult.Content.AddressStart)[0];
		array[31] = 0;
		array[32] = 4;
		array[33] = BitConverter.GetBytes(num * 8)[1];
		array[34] = BitConverter.GetBytes(num * 8)[0];
		values.CopyTo(array, 35);
		int num2 = 0;
		for (int i = 4; i < array.Length - 2; i++)
		{
			num2 += array[i];
		}
		array[array.Length - 2] = BitConverter.GetBytes(num2)[0];
		array[array.Length - 1] = 22;
		return OperateResult.CreateSuccessResult(array);
	}

	public static string GetMsgFromStatus(byte code)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			byte.MaxValue => "No error", 
			1 => "Hardware fault", 
			3 => "Illegal object access", 
			5 => "Invalid address(incorrent variable address)", 
			6 => "Data type is not supported", 
			10 => "Object does not exist or length error", 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static string GetMsgFromStatus(byte errorClass, byte errorCode)
	{
		if (errorClass == 128 && errorCode == 1)
		{
			return "Switch\u2002in\u2002wrong\u2002position\u2002for\u2002requested\u2002operation";
		}
		if (errorClass == 129 && errorCode == 4)
		{
			return "Miscellaneous\u2002structure\u2002error\u2002in\u2002command.\u2002\u2002Command is not supportedby CPU";
		}
		if (errorClass == 132 && errorCode == 4)
		{
			return "CPU is busy processing an upload or download CPU cannot process command because of system fault condition";
		}
		if (errorClass == 133 && errorCode == 0)
		{
			return "Length fields are not correct or do not agree with the amount of data received";
		}
		switch (errorClass)
		{
		case 210:
			return "Error in upload or download command";
		case 214:
			return "Protection error(password)";
		case 220:
			if (errorCode == 1)
			{
				return "Error in time-of-day clock data";
			}
			break;
		}
		return StringResources.Language.UnknownError;
	}

	public static OperateResult<byte[]> BuildWriteCommand(byte station, string address, bool[] values)
	{
		OperateResult<S7AddressData> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		byte[] array = SoftBasic.BoolArrayToByte(values);
		byte[] array2 = new byte[37 + array.Length];
		array2[0] = 104;
		array2[1] = BitConverter.GetBytes(array2.Length - 6)[0];
		array2[2] = BitConverter.GetBytes(array2.Length - 6)[0];
		array2[3] = 104;
		array2[4] = station;
		array2[5] = 0;
		array2[6] = 124;
		array2[7] = 50;
		array2[8] = 1;
		array2[9] = 0;
		array2[10] = 0;
		array2[11] = 0;
		array2[12] = 0;
		array2[13] = 0;
		array2[14] = 14;
		array2[15] = 0;
		array2[16] = 5;
		array2[17] = 5;
		array2[18] = 1;
		array2[19] = 18;
		array2[20] = 10;
		array2[21] = 16;
		array2[22] = 1;
		array2[23] = 0;
		array2[24] = BitConverter.GetBytes(values.Length)[0];
		array2[25] = BitConverter.GetBytes(values.Length)[1];
		array2[26] = (byte)operateResult.Content.DbBlock;
		array2[27] = operateResult.Content.DataCode;
		array2[28] = BitConverter.GetBytes(operateResult.Content.AddressStart)[2];
		array2[29] = BitConverter.GetBytes(operateResult.Content.AddressStart)[1];
		array2[30] = BitConverter.GetBytes(operateResult.Content.AddressStart)[0];
		array2[31] = 0;
		array2[32] = 3;
		array2[33] = BitConverter.GetBytes(values.Length)[1];
		array2[34] = BitConverter.GetBytes(values.Length)[0];
		array.CopyTo(array2, 35);
		int num = 0;
		for (int i = 4; i < array2.Length - 2; i++)
		{
			num += array2[i];
		}
		array2[array2.Length - 2] = BitConverter.GetBytes(num)[0];
		array2[array2.Length - 1] = 22;
		return OperateResult.CreateSuccessResult(array2);
	}

	public static bool CheckReceiveDataComplete(MemoryStream ms)
	{
		byte[] array = ms.ToArray();
		if (array.Length == 0)
		{
			return false;
		}
		if (array.Length == 1 && array[0] == 229)
		{
			return true;
		}
		if (array.Length > 6 && array[0] == 104 && array[1] + 6 == array.Length && array[array.Length - 1] == 22)
		{
			return true;
		}
		if (array.Length > 6 && array[0] == 3 && array[3] == array.Length)
		{
			return true;
		}
		return false;
	}

	public static OperateResult CheckResponse(byte[] content)
	{
		if (content.Length < 21)
		{
			return new OperateResult(10000, "Failed, data too short:" + SoftBasic.ByteToHexString(content, ' '));
		}
		if (content[17] != 0 || content[18] != 0)
		{
			return new OperateResult(content[19], GetMsgFromStatus(content[18], content[19]));
		}
		if (content.Length < 22)
		{
			return new OperateResult(10000, "Failed, data too short:" + SoftBasic.ByteToHexString(content, ' '));
		}
		if (content[21] != byte.MaxValue)
		{
			return new OperateResult(content[21], GetMsgFromStatus(content[21]));
		}
		return OperateResult.CreateSuccessResult();
	}

	public static byte[] GetExecuteConfirm(byte station)
	{
		byte[] array = new byte[6] { 16, 2, 0, 92, 94, 22 };
		array[1] = station;
		int num = 0;
		for (int i = 1; i < 4; i++)
		{
			num += array[i];
		}
		array[4] = (byte)num;
		return array;
	}

	private static OperateResult<byte[]> Read(IReadWriteDevice plc, S7AddressData address, ushort length, byte station, object communicationLock)
	{
		OperateResult<byte[]> operateResult = BuildReadCommand(station, address, length, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		lock (communicationLock)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			if (operateResult2.Content == null || operateResult2.Content.Length == 0 || operateResult2.Content[0] != 229)
			{
				return new OperateResult<byte[]>("PLC Receive Check Failed:" + SoftBasic.ByteToHexString(operateResult2.Content, ' '));
			}
			OperateResult<byte[]> operateResult3 = plc.ReadFromCoreServer(GetExecuteConfirm(station));
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			OperateResult operateResult4 = CheckResponse(operateResult3.Content);
			if (!operateResult4.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult4);
			}
			return SiemensS7Helper.AnalysisReadByte(operateResult3.Content);
		}
	}

	public static OperateResult<byte[]> Read(IReadWriteDevice plc, string address, ushort length, byte station, object communicationLock)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<S7AddressData> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return Read(plc, operateResult.Content, length, station2, communicationLock);
	}

	public static OperateResult<bool> ReadBool(IReadWriteDevice plc, string address, byte station, object communicationLock)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = BuildReadCommand(station2, address, 1, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult);
		}
		lock (communicationLock)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool>(operateResult2);
			}
			if (operateResult2.Content == null || operateResult2.Content.Length == 0 || operateResult2.Content[0] != 229)
			{
				return new OperateResult<bool>("PLC Receive Check Failed:" + SoftBasic.ByteToHexString(operateResult2.Content, ' '));
			}
			OperateResult<byte[]> operateResult3 = plc.ReadFromCoreServer(GetExecuteConfirm(station2));
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool>(operateResult3);
			}
			OperateResult operateResult4 = CheckResponse(operateResult3.Content);
			if (!operateResult4.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool>(operateResult4);
			}
			OperateResult<byte[]> operateResult5 = SiemensS7Helper.AnalysisReadBit(operateResult3.Content);
			if (!operateResult5.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool>(operateResult5);
			}
			return OperateResult.CreateSuccessResult(operateResult5.Content.ToBoolArray()[0]);
		}
	}

	public static OperateResult<bool[]> ReadBool(IReadWriteDevice plc, string address, ushort length, byte station, object communicationLock)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<S7AddressData> operateResult = AnalysisAddress(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		HslHelper.CalculateStartBitIndexAndLength(operateResult.Content.AddressStart, length, out var newStart, out var byteLength, out var offset);
		operateResult.Content.AddressStart = newStart;
		operateResult.Content.Length = byteLength;
		OperateResult<byte[]> operateResult2 = Read(plc, operateResult.Content, operateResult.Content.Length, station2, communicationLock);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.ToBoolArray().SelectMiddle(offset, length));
	}

	public static OperateResult Write(IReadWriteDevice plc, string address, byte[] value, byte station, object communicationLock)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = BuildWriteCommand(station2, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		lock (communicationLock)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			if (operateResult2.Content == null || operateResult2.Content.Length == 0 || operateResult2.Content[0] != 229)
			{
				return new OperateResult<byte[]>("PLC Receive Check Failed:" + SoftBasic.ByteToHexString(operateResult2.Content, ' '));
			}
			OperateResult<byte[]> operateResult3 = plc.ReadFromCoreServer(GetExecuteConfirm(station2));
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			OperateResult operateResult4 = CheckResponse(operateResult3.Content);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			return OperateResult.CreateSuccessResult();
		}
	}

	public static OperateResult Write(IReadWriteDevice plc, string address, bool[] value, byte station, object communicationLock)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		OperateResult<byte[]> operateResult = BuildWriteCommand(station2, address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		lock (communicationLock)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			if (operateResult2.Content == null || operateResult2.Content.Length == 0 || operateResult2.Content[0] != 229)
			{
				return new OperateResult<byte[]>("PLC Receive Check Failed:" + SoftBasic.ByteToHexString(operateResult2.Content, ' '));
			}
			OperateResult<byte[]> operateResult3 = plc.ReadFromCoreServer(GetExecuteConfirm(station2));
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
			OperateResult operateResult4 = CheckResponse(operateResult3.Content);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			return OperateResult.CreateSuccessResult();
		}
	}

	public static OperateResult Start(IReadWriteDevice plc, string parameter, byte station, object communicationLock)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref parameter, "s", station);
		byte[] array = new byte[39]
		{
			104, 33, 33, 104, 0, 0, 108, 50, 1, 0,
			0, 0, 0, 0, 20, 0, 0, 40, 0, 0,
			0, 0, 0, 0, 253, 0, 0, 9, 80, 95,
			80, 82, 79, 71, 82, 65, 77, 170, 22
		};
		array[4] = station;
		byte[] send = array;
		lock (communicationLock)
		{
			OperateResult<byte[]> operateResult = plc.ReadFromCoreServer(send);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			if (operateResult.Content == null || operateResult.Content.Length == 0 || operateResult.Content[0] != 229)
			{
				return new OperateResult<byte[]>("PLC Receive Check Failed:" + SoftBasic.ByteToHexString(operateResult.Content, ' '));
			}
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(GetExecuteConfirm(station2));
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			return OperateResult.CreateSuccessResult();
		}
	}

	public static OperateResult Stop(IReadWriteDevice plc, string parameter, byte station, object communicationLock)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref parameter, "s", station);
		byte[] array = new byte[35]
		{
			104, 29, 29, 104, 0, 0, 108, 50, 1, 0,
			0, 0, 0, 0, 16, 0, 0, 41, 0, 0,
			0, 0, 0, 9, 80, 95, 80, 82, 79, 71,
			82, 65, 77, 170, 22
		};
		array[4] = station;
		byte[] send = array;
		lock (communicationLock)
		{
			OperateResult<byte[]> operateResult = plc.ReadFromCoreServer(send);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			if (operateResult.Content == null || operateResult.Content.Length == 0 || operateResult.Content[0] != 229)
			{
				return new OperateResult<byte[]>("PLC Receive Check Failed:" + SoftBasic.ByteToHexString(operateResult.Content, ' '));
			}
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(GetExecuteConfirm(station2));
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			return OperateResult.CreateSuccessResult();
		}
	}

	public static OperateResult<string> ReadPlcType(IReadWriteDevice plc, string parameter, byte station, object communicationLock)
	{
		byte station2 = (byte)HslHelper.ExtractParameter(ref parameter, "s", station);
		OperateResult<byte[]> operateResult = BuildReadCommand(station2, "SYS0", 20, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		lock (communicationLock)
		{
			OperateResult<byte[]> operateResult2 = plc.ReadFromCoreServer(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult2);
			}
			if (operateResult2.Content == null || operateResult2.Content.Length == 0 || operateResult2.Content[0] != 229)
			{
				return new OperateResult<string>("PLC Receive Check Failed:" + SoftBasic.ByteToHexString(operateResult2.Content, ' '));
			}
			OperateResult<byte[]> operateResult3 = plc.ReadFromCoreServer(GetExecuteConfirm(station2));
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult3);
			}
			try
			{
				byte[] array = new byte[20];
				if (operateResult3.Content[21] == byte.MaxValue && operateResult3.Content[22] == 4)
				{
					Array.Copy(operateResult3.Content, 25, array, 0, 20);
				}
				return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(array));
			}
			catch (Exception ex)
			{
				return new OperateResult<string>("Get plc type failed: " + ex.Message + Environment.NewLine + "Content: " + operateResult3.Content.ToHexString(' '));
			}
		}
	}
}
