using System;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;

namespace HslCommunication.Profinet.Special;

public class EcFanMachine : DeviceSerialPort
{
	public byte Station { get; set; } = 1;

	protected override INetMessage GetNewNetMessage()
	{
		return new EuFunMessage();
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		if (response == null || response.Length < 8)
		{
			return new OperateResult<byte[]>("Command is too short: " + response.ToHexString(' '));
		}
		if (response.Length >= 9 && response[0] == byte.MaxValue && response[8] == 13)
		{
			response = response.RemoveBegin(1);
		}
		byte[] array = CalculateCheck(response);
		if (CheckCrc(response))
		{
			return OperateResult.CreateSuccessResult(response);
		}
		byte[] array2 = response.CopyArray();
		if (array2[0] != 126)
		{
			array2[0] = 126;
			if (CheckCrc(array2))
			{
				return OperateResult.CreateSuccessResult(response);
			}
		}
		return new OperateResult<byte[]>("Check response crc error: " + response.ToHexString(' '));
	}

	public OperateResult<EcFanData> ControlSpeed(bool run, bool emergency, int speed)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(BuildCommand5A(Station, run, emergency, speed));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<EcFanData>(operateResult);
		}
		return ParseResult5A(operateResult.Content);
	}

	public OperateResult SetStation(byte id)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(BuildCommandA2(Station, id));
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		Station = id;
		return OperateResult.CreateSuccessResult();
	}

	public OperateResult<int> ReadSpeedEmergency()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(BuildReadSpeed(173, Station));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(operateResult);
		}
		return OperateResult.CreateSuccessResult((int)BitConverter.ToUInt16(operateResult.Content, 3));
	}

	public OperateResult<int> ReadSpeedMin()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(BuildReadSpeed(174, Station));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(operateResult);
		}
		return OperateResult.CreateSuccessResult((int)BitConverter.ToUInt16(operateResult.Content, 3));
	}

	public OperateResult<int> ReadSpeedMax()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(BuildReadSpeed(175, Station));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(operateResult);
		}
		return OperateResult.CreateSuccessResult((int)BitConverter.ToUInt16(operateResult.Content, 3));
	}

	public static byte[] BuildCommand5A(byte stat, bool run, bool emergency, int speed)
	{
		byte[] array = new byte[4] { 90, stat, 0, 0 };
		if (run)
		{
			array[2] |= 128;
		}
		if (emergency)
		{
			array[2] |= 64;
		}
		byte[] bytes = BitConverter.GetBytes(speed);
		array[2] |= (byte)(bytes[1] & 0xF);
		array[3] = bytes[0];
		return BuildEntireCommand(array);
	}

	internal static OperateResult<EcFanData> ParseResult5A(byte[] command)
	{
		try
		{
			EcFanData ecFanData = new EcFanData();
			ecFanData.EmergencyState = (command[2] & 0x40) == 64;
			ecFanData.RunState = (command[3] & 0x80) == 128;
			ecFanData.LockState = (command[3] & 0x40) == 64;
			ecFanData.OverHotState = (command[3] & 0x20) == 32;
			ecFanData.LostSpeedState = (command[3] & 0x10) == 16;
			ecFanData.Speed = ((command[3] & 0xF) << 8) | command[4];
			return OperateResult.CreateSuccessResult(ecFanData);
		}
		catch (Exception ex)
		{
			return new OperateResult<EcFanData>("Parse result error: " + ex.Message + Environment.NewLine + "Source Code: " + command.ToHexString(' '));
		}
	}

	internal static byte[] BuildCommandA2(byte stat, byte id)
	{
		return BuildEntireCommand(new byte[4] { 162, stat, id, 255 });
	}

	internal static byte[] BuildReadSpeed(byte code, byte stat)
	{
		return BuildEntireCommand(new byte[4] { code, stat, 44, 1 });
	}

	public static byte[] BuildEntireCommand(byte[] command)
	{
		byte[] array = new byte[command.Length + 4];
		array[0] = 126;
		command.CopyTo(array, 1);
		byte[] array2 = CalculateCheck(array);
		array[command.Length + 1] = array2[0];
		array[command.Length + 2] = array2[1];
		array[command.Length + 3] = 13;
		return array;
	}

	internal static byte[] CalculateCheck(byte[] command)
	{
		int num = 0;
		for (int i = 0; i < command.Length - 3 && i < 5; i++)
		{
			num += command[i];
		}
		num = ~num + 1;
		return new byte[2]
		{
			(byte)(num >> 8),
			(byte)(num & 0xFF)
		};
	}

	internal static bool CheckCrc(byte[] command)
	{
		if (command == null || command.Length < 8)
		{
			return false;
		}
		byte[] array = CalculateCheck(command);
		return command[5] == array[0] && command[6] == array[1];
	}

	public static bool CheckReceiveDataComplete(byte[] send, byte[] response)
	{
		if (response == null)
		{
			return false;
		}
		if (response.Length >= 8 && response[7] == 13)
		{
			return true;
		}
		if (response.Length >= 9 && response[0] == byte.MaxValue && response[8] == 13)
		{
			return true;
		}
		return false;
	}
}
