using System;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.ModBus;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.XINJE;

public class XinJEServer : ModbusTcpServer
{
	private SoftBuffer mBuffer;

	private SoftBuffer smBuffer;

	private SoftBuffer dBuffer;

	private SoftBuffer sdBuffer;

	private SoftBuffer hdBuffer;

	private const int DataPoolLength = 65536;

	public XinJEServer()
	{
		mBuffer = new SoftBuffer(65536);
		smBuffer = new SoftBuffer(65536);
		dBuffer = new SoftBuffer(1000000);
		sdBuffer = new SoftBuffer(131072);
		hdBuffer = new SoftBuffer(131072);
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		try
		{
			if (address.StartsWithAndNumber("D"))
			{
				return OperateResult.CreateSuccessResult(dBuffer.GetBytes(Convert.ToInt32(address.Substring(1)) * 2, length * 2));
			}
			if (address.StartsWithAndNumber("SD"))
			{
				return OperateResult.CreateSuccessResult(sdBuffer.GetBytes(Convert.ToInt32(address.Substring(2)) * 2, length * 2));
			}
			if (address.StartsWithAndNumber("HD"))
			{
				return OperateResult.CreateSuccessResult(hdBuffer.GetBytes(Convert.ToInt32(address.Substring(2)) * 2, length * 2));
			}
			return base.Read(address, length);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		try
		{
			if (address.StartsWithAndNumber("D"))
			{
				dBuffer.SetBytes(value, Convert.ToInt32(address.Substring(1)) * 2);
				return OperateResult.CreateSuccessResult();
			}
			if (address.StartsWithAndNumber("SD"))
			{
				sdBuffer.SetBytes(value, Convert.ToInt32(address.Substring(2)) * 2);
				return OperateResult.CreateSuccessResult();
			}
			if (address.StartsWithAndNumber("HD"))
			{
				hdBuffer.SetBytes(value, Convert.ToInt32(address.Substring(2)) * 2);
				return OperateResult.CreateSuccessResult();
			}
			return base.Write(address, value);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		try
		{
			if (address.StartsWithAndNumber("M"))
			{
				return OperateResult.CreateSuccessResult(mBuffer.GetBool(Convert.ToInt32(address.Substring(1)), length));
			}
			if (address.StartsWithAndNumber("SM"))
			{
				return OperateResult.CreateSuccessResult(smBuffer.GetBool(Convert.ToInt32(address.Substring(2)), length));
			}
			return base.ReadBool(address, length);
		}
		catch (Exception ex)
		{
			return new OperateResult<bool[]>(ex.Message);
		}
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		try
		{
			if (address.StartsWithAndNumber("M"))
			{
				mBuffer.SetBool(value, Convert.ToInt32(address.Substring(1)));
				return OperateResult.CreateSuccessResult();
			}
			if (address.StartsWithAndNumber("SM"))
			{
				smBuffer.SetBool(value, Convert.ToInt32(address.Substring(2)));
				return OperateResult.CreateSuccessResult();
			}
			return base.Write(address, value);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	protected override OperateResult<byte[]> ReadFromCoreServer(PipeSession session, byte[] receive)
	{
		if (receive[7] == 32 || receive[7] == 30)
		{
			if (!base.StationDataIsolation && base.StationCheck && receive[6] != base.Station)
			{
				return new OperateResult<byte[]>($"Station not match XinJe, Need {base.Station} actual {receive[6]}");
			}
			return OperateResult.CreateSuccessResult(ReadByCommand(receive));
		}
		if (receive[7] == 33 || receive[7] == 31)
		{
			if (!base.StationDataIsolation && base.StationCheck && receive[6] != base.Station)
			{
				return new OperateResult<byte[]>($"Station not match XinJe, Need {base.Station} actual {receive[6]}");
			}
			return OperateResult.CreateSuccessResult(WriteByMessage(receive));
		}
		return base.ReadFromCoreServer(session, receive);
	}

	private byte[] PackCommand(byte[] command, ushort status, byte[] data)
	{
		if (data == null)
		{
			byte[] array = command.SelectBegin(14);
			array[4] = 0;
			array[5] = 8;
			if (status == 0)
			{
				return array;
			}
			array[7] = (byte)(array[7] + 128 + status);
			return array;
		}
		byte[] array2 = new byte[9 + data.Length];
		Array.Copy(command, 0, array2, 0, 8);
		array2[4] = 0;
		array2[5] = (byte)(array2.Length - 6);
		array2[8] = (byte)data.Length;
		data.CopyTo(array2, 9);
		return array2;
	}

	private byte[] ReadByCommand(byte[] command)
	{
		ushort num = base.ByteTransform.TransUInt16(command, 12);
		int num2 = command[9] * 65536 + command[10] * 256 + command[11];
		byte b = command[8];
		if (command[7] == 32)
		{
			if (num > 125)
			{
				return PackCommand(command, 1, null);
			}
			if (1 == 0)
			{
			}
			byte[] result = b switch
			{
				128 => PackCommand(command, 0, dBuffer.GetBytes(num2 * 2, num * 2)), 
				131 => PackCommand(command, 0, sdBuffer.GetBytes(num2 * 2, num * 2)), 
				136 => PackCommand(command, 0, hdBuffer.GetBytes(num2 * 2, num * 2)), 
				_ => PackCommand(command, 1, null), 
			};
			if (1 == 0)
			{
			}
			return result;
		}
		if (command[7] == 30)
		{
			if (num > 2000)
			{
				return PackCommand(command, 1, null);
			}
			if (1 == 0)
			{
			}
			byte[] result = b switch
			{
				3 => PackCommand(command, 0, mBuffer.GetBool(num2, num).ToByteArray()), 
				13 => PackCommand(command, 0, smBuffer.GetBool(num2, num).ToByteArray()), 
				_ => PackCommand(command, 1, null), 
			};
			if (1 == 0)
			{
			}
			return result;
		}
		return PackCommand(command, 1, null);
	}

	private byte[] WriteByMessage(byte[] command)
	{
		if (!base.EnableWrite)
		{
			return PackCommand(command, 1, null);
		}
		ushort length = base.ByteTransform.TransUInt16(command, 12);
		int num = command[9] * 65536 + command[10] * 256 + command[11];
		byte b = command[8];
		if (command[7] == 33)
		{
			byte[] data = command.SelectMiddle(15, command[14]);
			switch (b)
			{
			case 128:
				dBuffer.SetBytes(data, num * 2);
				return PackCommand(command, 0, null);
			case 131:
				sdBuffer.SetBytes(data, num * 2);
				return PackCommand(command, 0, null);
			case 136:
				hdBuffer.SetBytes(data, num * 2);
				return PackCommand(command, 0, null);
			default:
				return PackCommand(command, 1, null);
			}
		}
		if (command[7] == 31)
		{
			bool[] value = command.SelectMiddle(15, command[14]).ToBoolArray().SelectBegin(length);
			switch (b)
			{
			case 3:
				mBuffer.SetBool(value, num);
				return PackCommand(command, 0, null);
			case 13:
				smBuffer.SetBool(value, num);
				return PackCommand(command, 0, null);
			default:
				return PackCommand(command, 1, null);
			}
		}
		return PackCommand(command, 1, null);
	}

	public override string ToString()
	{
		return $"XinJEServer[{base.Port}]";
	}
}
