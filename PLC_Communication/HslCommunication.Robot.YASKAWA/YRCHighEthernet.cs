using System;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;
using HslCommunication.Robot.YASKAWA.Helper;

namespace HslCommunication.Robot.YASKAWA;

public class YRCHighEthernet : NetworkUdpBase
{
	private IByteTransform byteTransform = new RegularByteTransform();

	private SoftIncrementCount incrementCount = new SoftIncrementCount(255L, 0L);

	private byte handle = 1;

	private Encoding encoding = Encoding.ASCII;

	public YRCHighEthernet()
	{
	}

	public YRCHighEthernet(string ipAddress, int port = 10040)
	{
		IpAddress = ipAddress;
		Port = port;
	}

	public OperateResult<byte[]> ReadCommand(ushort command, ushort dataAddress, byte dataAttribute, byte dataHandle, byte[] dataPart)
	{
		byte[] send = YRCHighEthernetHelper.BuildCommand(handle, (byte)incrementCount.GetCurrentValue(), command, dataAddress, dataAttribute, dataHandle, dataPart);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = YRCHighEthernetHelper.CheckResponseContent(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		if (operateResult.Content.Length > 32)
		{
			return OperateResult.CreateSuccessResult(operateResult.Content.RemoveBegin(32));
		}
		return OperateResult.CreateSuccessResult(new byte[0]);
	}

	public OperateResult<YRCAlarmItem[]> ReadAlarms()
	{
		YRCAlarmItem[] array = new YRCAlarmItem[4];
		for (int i = 0; i < array.Length; i++)
		{
			OperateResult<byte[]> operateResult = ReadCommand(112, (ushort)(i + 1), 0, 1, null);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<YRCAlarmItem[]>(operateResult);
			}
			if (operateResult.Content.Length != 0)
			{
				array[i] = new YRCAlarmItem(byteTransform, operateResult.Content, encoding);
			}
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public OperateResult<YRCAlarmItem[]> ReadHistoryAlarms(ushort alarmType, short length)
	{
		YRCAlarmItem[] array = new YRCAlarmItem[length];
		for (int i = 0; i < array.Length; i++)
		{
			OperateResult<byte[]> operateResult = ReadCommand(113, alarmType, 0, 1, null);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<YRCAlarmItem[]>(operateResult);
			}
			if (operateResult.Content.Length != 0)
			{
				array[i] = new YRCAlarmItem(byteTransform, operateResult.Content, encoding);
			}
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public OperateResult<bool[]> ReadStats()
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(114, 1, 0, 1, null), (byte[] m) => new byte[2]
		{
			(byte)byteTransform.TransInt32(m, 0),
			(byte)byteTransform.TransInt32(m, 4)
		}.ToBoolArray());
	}

	public OperateResult<string[]> ReadJSeq(ushort task = 1)
	{
		OperateResult<byte[]> operateResult = ReadCommand(115, task, 0, 1, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(new string[4]
		{
			encoding.GetString(operateResult.Content, 0, 32),
			byteTransform.TransInt32(operateResult.Content, 32).ToString(),
			byteTransform.TransInt32(operateResult.Content, 36).ToString(),
			byteTransform.TransInt32(operateResult.Content, 40).ToString()
		});
	}

	public OperateResult<string[]> ReadPose()
	{
		OperateResult<byte[]> operateResult = ReadCommand(117, 101, 0, 1, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		string[] array = new string[operateResult.Content.Length / 4 - 5];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = byteTransform.TransInt32(operateResult.Content, 20 + i * 4).ToString();
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public OperateResult<string[]> ReadTorqueData()
	{
		OperateResult<byte[]> operateResult = ReadCommand(119, 21, 0, 1, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		string[] array = new string[operateResult.Content.Length / 4];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = byteTransform.TransInt32(operateResult.Content, i * 4).ToString();
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public OperateResult<byte> ReadIO(ushort address)
	{
		OperateResult<byte[]> operateResult = ReadCommand(120, address, 1, 14, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content[0]);
	}

	public OperateResult WriteIO(ushort address, byte value)
	{
		return ReadCommand(120, address, 1, 16, new byte[1] { value });
	}

	public OperateResult<byte[]> ReadIO(ushort address, int length)
	{
		OperateResult<byte[]> operateResult = ReadCommand(768, address, 0, 51, byteTransform.TransByte(length));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		int length2 = byteTransform.TransInt32(operateResult.Content, 0);
		return OperateResult.CreateSuccessResult(operateResult.Content.SelectMiddle(4, length2));
	}

	public OperateResult WriteIO(ushort address, byte[] value)
	{
		return ReadCommand(768, address, 0, 52, value);
	}

	public OperateResult<ushort> ReadRegisterVariable(ushort address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(121, address, 1, 14, null), (byte[] m) => byteTransform.TransUInt16(m, 0));
	}

	public OperateResult WriteRegisterVariable(ushort address, ushort value)
	{
		return ReadCommand(121, address, 1, 16, byteTransform.TransByte(value));
	}

	public OperateResult<ushort[]> ReadRegisterVariable(ushort address, int length)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(769, address, 0, 51, byteTransform.TransByte(length)), (byte[] m) => byteTransform.TransUInt16(m, 0, length));
	}

	public OperateResult WriteRegisterVariable(ushort address, ushort[] value)
	{
		return ReadCommand(769, address, 0, 52, byteTransform.TransByte(value));
	}

	public OperateResult<byte> ReadByteVariable(ushort address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadCommand(122, address, 1, 14, null));
	}

	public OperateResult WriteByteVariable(ushort address, byte value)
	{
		return ReadCommand(122, address, 1, 16, new byte[1] { value });
	}

	public OperateResult<byte[]> ReadByteVariable(ushort address, int length)
	{
		return ReadCommand(770, address, 0, 51, byteTransform.TransByte(length));
	}

	public OperateResult WriteByteVariable(ushort address, byte[] vaule)
	{
		return ReadCommand(770, address, 0, 52, vaule);
	}

	public OperateResult<short> ReadIntegerVariable(ushort address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(123, address, 1, 14, null), (byte[] m) => byteTransform.TransInt16(m, 0));
	}

	public OperateResult WriteIntegerVariable(ushort address, short value)
	{
		return ReadCommand(123, address, 1, 16, byteTransform.TransByte(value));
	}

	public OperateResult<short[]> ReadIntegerVariable(ushort address, int length)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(771, address, 0, 51, byteTransform.TransByte(length)), (byte[] m) => byteTransform.TransInt16(m, 0, length));
	}

	public OperateResult WriteIntegerVariable(ushort address, short[] value)
	{
		return ReadCommand(771, address, 0, 52, byteTransform.TransByte(value));
	}

	public OperateResult<int> ReadDoubleIntegerVariable(ushort address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(124, address, 1, 14, null), (byte[] m) => byteTransform.TransInt32(m, 0));
	}

	public OperateResult WriteDoubleIntegerVariable(ushort address, int value)
	{
		return ReadCommand(124, address, 1, 16, byteTransform.TransByte(value));
	}

	public OperateResult<int[]> ReadDoubleIntegerVariable(ushort address, int length)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(772, address, 0, 51, byteTransform.TransByte(length)), (byte[] m) => byteTransform.TransInt32(m, 0, length));
	}

	public OperateResult WriteDoubleIntegerVariable(ushort address, int[] value)
	{
		return ReadCommand(772, address, 0, 52, byteTransform.TransByte(value));
	}

	public OperateResult<float> ReadRealVariable(ushort address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(125, address, 1, 14, null), (byte[] m) => byteTransform.TransSingle(m, 0));
	}

	public OperateResult WriteRealVariable(ushort address, float value)
	{
		return ReadCommand(125, address, 1, 16, byteTransform.TransByte(value));
	}

	public OperateResult<float[]> ReadRealVariable(ushort address, int length)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(773, address, 0, 51, byteTransform.TransByte(length)), (byte[] m) => byteTransform.TransSingle(m, 0, length));
	}

	public OperateResult WriteRealVariable(ushort address, float[] value)
	{
		return ReadCommand(773, address, 0, 52, byteTransform.TransByte(value));
	}

	public OperateResult<string> ReadStringVariable(ushort address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadCommand(126, address, 1, 14, null), (byte[] m) => byteTransform.TransString(m, encoding));
	}

	public OperateResult WriteStringVariable(ushort address, string value)
	{
		return ReadCommand(126, address, 1, 16, SoftBasic.ArrayExpandToLength(encoding.GetBytes(value), 16));
	}

	public OperateResult<string[]> ReadStringVariable(ushort address, int length)
	{
		OperateResult<byte[]> operateResult = ReadCommand(774, address, 0, 51, byteTransform.TransByte(length));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		string[] array = new string[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = encoding.GetString(operateResult.Content, i * 16, 16);
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public OperateResult WriteStringVariable(ushort address, string[] value)
	{
		byte[] array = new byte[value.Length * 16];
		for (int i = 0; i < value.Length; i++)
		{
			encoding.GetBytes(value[i]).CopyTo(array, i * 16);
		}
		return ReadCommand(774, address, 0, 52, array);
	}

	[HslMqttApi(Description = "进行HOLD 的 ON/OFF 操作，状态参数 False: OFF，True: ON")]
	public OperateResult Hold(bool status)
	{
		return ReadCommand(131, 1, 1, 16, status ? byteTransform.TransByte(1) : byteTransform.TransByte(2));
	}

	[HslMqttApi(Description = "对机械手的报警进行复位")]
	public OperateResult Reset()
	{
		return ReadCommand(130, 1, 1, 16, byteTransform.TransByte(1));
	}

	[HslMqttApi(Description = "进行错误取消")]
	public OperateResult Cancel()
	{
		return ReadCommand(130, 2, 1, 16, byteTransform.TransByte(1));
	}

	[HslMqttApi(Description = "进行伺服电源的ON/OFF操作，状态参数 False: OFF，True: ON")]
	public OperateResult Svon(bool status)
	{
		return ReadCommand(131, 2, 1, 16, status ? byteTransform.TransByte(1) : byteTransform.TransByte(2));
	}

	[HslMqttApi(Description = "设定示教编程器和 I/O的操作信号的联锁。 状态参数 False: OFF，True: ON")]
	public OperateResult HLock(bool status)
	{
		return ReadCommand(131, 3, 1, 16, status ? byteTransform.TransByte(1) : byteTransform.TransByte(2));
	}

	[HslMqttApi(Description = "选择循环。循环编号 1:步骤，2:1循环，3:连续自动")]
	public OperateResult Cycle(int number)
	{
		return ReadCommand(132, 2, 1, 16, byteTransform.TransByte(number));
	}

	[HslMqttApi(Description = "接受消息数据时， 在YRC1000的示教编程器的远程画面下显示消息若。若不是远程画面时，强制切换到远程画面。显示MDSP命令的消息。")]
	public OperateResult MSDP(string message)
	{
		return ReadCommand(133, 1, 1, 16, string.IsNullOrEmpty(message) ? new byte[0] : encoding.GetBytes(message));
	}

	[HslMqttApi(Description = "开始程序。操作时指定程序名时，此程序能附带对应主程序，则从该程序的开头开始执行。如果没有指定，则从前行开始执行")]
	public OperateResult Start()
	{
		return ReadCommand(134, 1, 1, 16, byteTransform.TransByte(1));
	}

	public OperateResult<DateTime> ReadManagementTime(ushort address)
	{
		OperateResult<byte[]> operateResult = ReadCommand(136, address, 1, 14, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Convert.ToDateTime(Encoding.ASCII.GetString(operateResult.Content, 0, 16)));
	}

	public OperateResult<string> ReadManagementTimeSpan(ushort address)
	{
		OperateResult<byte[]> operateResult = ReadCommand(136, address, 2, 14, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(operateResult.Content, 0, 12));
	}

	public OperateResult<string[]> ReadSystemInfo(ushort system)
	{
		OperateResult<byte[]> operateResult = ReadCommand(137, system, 0, 1, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(new string[3]
		{
			encoding.GetString(operateResult.Content, 0, 24),
			encoding.GetString(operateResult.Content, 24, 16),
			encoding.GetString(operateResult.Content, 40, 8)
		});
	}

	[HslMqttApi(Description = "设定执行程序的名称和行编号。")]
	public OperateResult JSeq(string programName, int line)
	{
		byte[] array = new byte[36];
		encoding.GetBytes(programName).CopyTo(array, 0);
		byteTransform.TransByte(line).CopyTo(array, 32);
		return ReadCommand(132, 2, 1, 16, array);
	}

	[HslMqttApi(Description = "指定坐标系的当前值读取。并且可以指定外部轴的有无。")]
	public OperateResult<YRCRobotData> ReadPOSC(int coordinate, bool hasExteralAxis)
	{
		return new OperateResult<YRCRobotData>();
	}
}
