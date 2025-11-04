using System;
using System.IO;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Core.Device;
using HslCommunication.Reflection;
using HslCommunication.Serial;

namespace HslCommunication.Profinet.AllenBradley;

public class AllenBradleyDF1Serial : DeviceSerialPort
{
	private SoftIncrementCount incrementCount;

	public byte Station { get; set; }

	public byte DstNode { get; set; }

	public byte SrcNode { get; set; }

	public CheckType CheckType { get; set; }

	public AllenBradleyDF1Serial()
	{
		base.WordLength = 2;
		base.ByteTransform = new RegularByteTransform();
		incrementCount = new SoftIncrementCount(65535L, 0L);
		CheckType = CheckType.CRC16;
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", Station);
		byte dstNode = (byte)HslHelper.ExtractParameter(ref address, "dst", DstNode);
		byte srcNode = (byte)HslHelper.ExtractParameter(ref address, "src", SrcNode);
		OperateResult<byte[]> operateResult = BuildProtectedTypedLogicalReadWithThreeAddressFields(dstNode, srcNode, (int)incrementCount.GetCurrentValue(), address, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(PackCommand(station, operateResult.Content));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return ExtractActualData(operateResult2.Content);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		byte station = (byte)HslHelper.ExtractParameter(ref address, "s", Station);
		byte dstNode = (byte)HslHelper.ExtractParameter(ref address, "dst", DstNode);
		byte srcNode = (byte)HslHelper.ExtractParameter(ref address, "src", SrcNode);
		OperateResult<byte[]> operateResult = BuildProtectedTypedLogicalWriteWithThreeAddressFields(dstNode, srcNode, (int)incrementCount.GetCurrentValue(), address, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(PackCommand(station, operateResult.Content));
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return ExtractActualData(operateResult2.Content);
	}

	private byte[] CalculateCheckResult(byte station, byte[] command)
	{
		if (CheckType == CheckType.BCC)
		{
			int num = station;
			for (int i = 0; i < command.Length; i++)
			{
				num += command[i];
			}
			num = (byte)(~num);
			num++;
			return new byte[1] { (byte)num };
		}
		byte[] value = SoftBasic.SpliceArray<byte>(new byte[1] { station }, new byte[1] { 2 }, command, new byte[1] { 3 });
		return SoftCRC16.CRC16(value, 160, 1, 0, 0).SelectLast(2);
	}

	public byte[] PackCommand(byte station, byte[] command)
	{
		byte[] array = CalculateCheckResult(station, command);
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(16);
		memoryStream.WriteByte(1);
		memoryStream.WriteByte(station);
		if (station == 16)
		{
			memoryStream.WriteByte(station);
		}
		memoryStream.WriteByte(16);
		memoryStream.WriteByte(2);
		for (int i = 0; i < command.Length; i++)
		{
			memoryStream.WriteByte(command[i]);
			if (command[i] == 16)
			{
				memoryStream.WriteByte(command[i]);
			}
		}
		memoryStream.WriteByte(16);
		memoryStream.WriteByte(3);
		memoryStream.Write(array, 0, array.Length);
		return memoryStream.ToArray();
	}

	private static void AddLengthToMemoryStream(MemoryStream ms, ushort value)
	{
		if (value < 255)
		{
			ms.WriteByte((byte)value);
			return;
		}
		ms.WriteByte(byte.MaxValue);
		ms.WriteByte(BitConverter.GetBytes(value)[0]);
		ms.WriteByte(BitConverter.GetBytes(value)[1]);
	}

	public static OperateResult<byte[]> BuildProtectedTypedLogicalReadWithThreeAddressFields(int tns, string address, int length)
	{
		OperateResult<AllenBradleySLCAddress> operateResult = AllenBradleySLCAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(15);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(BitConverter.GetBytes(tns)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(tns)[1]);
		memoryStream.WriteByte(162);
		memoryStream.WriteByte(BitConverter.GetBytes(length)[0]);
		AddLengthToMemoryStream(memoryStream, operateResult.Content.DbBlock);
		memoryStream.WriteByte(operateResult.Content.DataCode);
		AddLengthToMemoryStream(memoryStream, (ushort)operateResult.Content.AddressStart);
		AddLengthToMemoryStream(memoryStream, 0);
		return OperateResult.CreateSuccessResult(memoryStream.ToArray());
	}

	public static OperateResult<byte[]> BuildProtectedTypedLogicalReadWithThreeAddressFields(byte dstNode, byte srcNode, int tns, string address, int length)
	{
		OperateResult<byte[]> operateResult = BuildProtectedTypedLogicalReadWithThreeAddressFields(tns, address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<byte>(new byte[2] { dstNode, srcNode }, operateResult.Content));
	}

	public static OperateResult<byte[]> BuildProtectedTypedLogicalWriteWithThreeAddressFields(int tns, string address, byte[] data)
	{
		OperateResult<AllenBradleySLCAddress> operateResult = AllenBradleySLCAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(15);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(BitConverter.GetBytes(tns)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(tns)[1]);
		memoryStream.WriteByte(170);
		memoryStream.WriteByte(BitConverter.GetBytes(data.Length)[0]);
		AddLengthToMemoryStream(memoryStream, operateResult.Content.DbBlock);
		memoryStream.WriteByte(operateResult.Content.DataCode);
		AddLengthToMemoryStream(memoryStream, (ushort)operateResult.Content.AddressStart);
		AddLengthToMemoryStream(memoryStream, 0);
		memoryStream.Write(data);
		return OperateResult.CreateSuccessResult(memoryStream.ToArray());
	}

	public static OperateResult<byte[]> BuildProtectedTypedLogicalWriteWithThreeAddressFields(byte dstNode, byte srcNode, int tns, string address, byte[] data)
	{
		OperateResult<byte[]> operateResult = BuildProtectedTypedLogicalWriteWithThreeAddressFields(tns, address, data);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<byte>(new byte[2] { dstNode, srcNode }, operateResult.Content));
	}

	public static OperateResult<byte[]> BuildProtectedTypedLogicalMaskWithThreeAddressFields(int tns, string address, int bitIndex, bool value)
	{
		int value2 = 1 << bitIndex;
		OperateResult<AllenBradleySLCAddress> operateResult = AllenBradleySLCAddress.ParseFrom(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(15);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(BitConverter.GetBytes(tns)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(tns)[1]);
		memoryStream.WriteByte(171);
		memoryStream.WriteByte(2);
		AddLengthToMemoryStream(memoryStream, operateResult.Content.DbBlock);
		memoryStream.WriteByte(operateResult.Content.DataCode);
		AddLengthToMemoryStream(memoryStream, (ushort)operateResult.Content.AddressStart);
		AddLengthToMemoryStream(memoryStream, 0);
		memoryStream.WriteByte(BitConverter.GetBytes(value2)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(value2)[1]);
		if (value)
		{
			memoryStream.WriteByte(BitConverter.GetBytes(value2)[0]);
			memoryStream.WriteByte(BitConverter.GetBytes(value2)[1]);
		}
		else
		{
			memoryStream.WriteByte(0);
			memoryStream.WriteByte(0);
		}
		return OperateResult.CreateSuccessResult(memoryStream.ToArray());
	}

	public static OperateResult<byte[]> ExtractActualData(byte[] content)
	{
		try
		{
			int num = -1;
			for (int i = 0; i < content.Length; i++)
			{
				if (content[i] == 16 && content[i + 1] == 2)
				{
					num = i + 2;
					break;
				}
			}
			if (num < 0 || num >= content.Length - 6)
			{
				return new OperateResult<byte[]>("Message must start with '10 02', source: " + content.ToHexString(' '));
			}
			MemoryStream memoryStream = new MemoryStream();
			for (int j = num; j < content.Length - 1; j++)
			{
				if (content[j] == 16 && content[j + 1] == 16)
				{
					memoryStream.WriteByte(content[j]);
					j++;
					continue;
				}
				if (content[j] == 16 && content[j + 1] == 3)
				{
					break;
				}
				memoryStream.WriteByte(content[j]);
			}
			content = memoryStream.ToArray();
			if (content[3] == 240)
			{
				return new OperateResult<byte[]>(GetExtStatusDescription(content[6]));
			}
			if (content[3] != 0)
			{
				return new OperateResult<byte[]>(GetStatusDescription(content[3]));
			}
			if (content.Length > 6)
			{
				return OperateResult.CreateSuccessResult(content.RemoveBegin(6));
			}
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message + " Source:" + content.ToHexString(' '));
		}
	}

	public static string GetStatusDescription(byte code)
	{
		byte b = (byte)(code & 0xF);
		byte b2 = (byte)(code & 0xF0);
		if (1 == 0)
		{
		}
		string result;
		switch (b)
		{
		case 1:
			result = "DST node is out of buffer space";
			break;
		case 2:
			result = "Cannot guarantee delivery: link layer(The remote node specified does not ACK command.)";
			break;
		case 3:
			result = "Duplicate token holder detected";
			break;
		case 4:
			result = "Local port is disconnected";
			break;
		case 5:
			result = "Application layer timed out waiting for a response";
			break;
		case 6:
			result = "Duplicate node detected";
			break;
		case 7:
			result = "Station is offline";
			break;
		case 8:
			result = "Hardware fault";
			break;
		default:
		{
			if (1 == 0)
			{
			}
			string text = b2 switch
			{
				16 => "Illegal command or format", 
				32 => "Host has a problem and will not communicate", 
				48 => "Remote node host is missing, disconnected, or shut down", 
				64 => "Host could not complete function due to hardware fault", 
				80 => "Addressing problem or memory protect rungs", 
				96 => "Function not allowed due to command protection selection", 
				112 => "Processor is in Program mode", 
				128 => "Compatibility mode file missing or communication zone problem", 
				144 => "Remote node cannot buffer command", 
				160 => "Wait ACK (1775\u0006KA buffer full)", 
				176 => "Remote node problem due to download", 
				192 => "Wait ACK (1775\u0006KA buffer full)", 
				240 => "Error code in the EXT STS byte", 
				_ => StringResources.Language.UnknownError, 
			};
			if (1 == 0)
			{
			}
			result = text;
			break;
		}
		}
		if (1 == 0)
		{
		}
		return result;
	}

	public static string GetExtStatusDescription(byte code)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			1 => "A field has an illegal value", 
			2 => "Less levels specified in address than minimum for any address", 
			3 => "More levels specified in address than system supports", 
			4 => "Symbol not found", 
			5 => "Symbol is of improper format", 
			6 => "Address doesn’t point to something usable", 
			7 => "File is wrong size", 
			8 => "Cannot complete request, situation has changed since the start of the command", 
			9 => "Data or file is too large", 
			10 => "Transaction size plus word address is too large", 
			11 => "Access denied, improper privilege", 
			12 => "Condition cannot be generated \u0006 resource is not available", 
			13 => "Condition already exists \u0006 resource is already available", 
			14 => "Command cannot be executed", 
			15 => "Histogram overflow", 
			16 => "No access", 
			17 => "Illegal data type", 
			18 => "Invalid parameter or invalid data", 
			19 => "Address reference exists to deleted area", 
			20 => "Command execution failure for unknown reason; possible PLC\u00063 histogram overflow", 
			21 => "Data conversion error", 
			22 => "Scanner not able to communicate with 1771 rack adapter", 
			23 => "Type mismatch", 
			24 => "1771 module response was not valid", 
			25 => "Duplicated label", 
			26 => "File is open; another node owns it", 
			27 => "Another node is the program owner", 
			28 => "Reserved", 
			29 => "Reserved", 
			30 => "Data table element protection violation", 
			31 => "Temporary internal problem", 
			34 => "Remote rack fault", 
			35 => "Timeout", 
			36 => "Unknown error", 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public override string ToString()
	{
		return $"AllenBradleyDF1Serial[{base.PortName}:{base.BaudRate}]";
	}
}
