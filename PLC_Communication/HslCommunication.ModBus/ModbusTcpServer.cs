using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Reflection;
using HslCommunication.Serial;

namespace HslCommunication.ModBus;

public class ModbusTcpServer : DeviceServer
{
	private List<ModBusMonitorAddress> subscriptions;

	private SimpleHybirdLock subcriptionHybirdLock;

	private ModbusDataDict dictModbusDataPool;

	private byte station = 1;

	private bool stationDataIsolation = false;

	private IByteTransform byteTransformSelf = new RegularByteTransform(DataFormat.CDAB);

	private SoftBuffer fileBuffer = new SoftBuffer(65535);

	public DataFormat DataFormat
	{
		get
		{
			return base.ByteTransform.DataFormat;
		}
		set
		{
			base.ByteTransform.DataFormat = value;
		}
	}

	public bool IsStringReverse
	{
		get
		{
			return base.ByteTransform.IsStringReverseByteWord;
		}
		set
		{
			base.ByteTransform.IsStringReverseByteWord = value;
		}
	}

	public byte Station
	{
		get
		{
			return station;
		}
		set
		{
			station = value;
		}
	}

	public bool StationCheck { get; set; } = true;

	public bool UseModbusRtuOverTcp { get; set; }

	public int RequestDelayTime { get; set; }

	public bool EnableWriteMaskCode { get; set; } = true;

	public bool StationDataIsolation
	{
		get
		{
			return stationDataIsolation;
		}
		set
		{
			stationDataIsolation = value;
			dictModbusDataPool.Set(value);
		}
	}

	public ModbusTcpServer()
	{
		dictModbusDataPool = new ModbusDataDict();
		subscriptions = new List<ModBusMonitorAddress>();
		subcriptionHybirdLock = new SimpleHybirdLock();
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
		base.WordLength = 1;
	}

	protected override byte[] SaveToBytes()
	{
		return dictModbusDataPool.GetModbusPool(station).SaveToBytes();
	}

	protected override void LoadFromBytes(byte[] content)
	{
		dictModbusDataPool.GetModbusPool(station).LoadFromBytes(content, 0);
	}

	public bool ReadCoil(string address)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		return dictModbusDataPool.GetModbusPool(b).ReadCoil(address);
	}

	public bool[] ReadCoil(string address, ushort length)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		return dictModbusDataPool.GetModbusPool(b).ReadCoil(address, length);
	}

	public void WriteCoil(string address, bool data)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		dictModbusDataPool.GetModbusPool(b).WriteCoil(address, data);
	}

	public void WriteCoil(string address, bool[] data)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		dictModbusDataPool.GetModbusPool(b).WriteCoil(address, data);
	}

	public bool ReadDiscrete(string address)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		return dictModbusDataPool.GetModbusPool(b).ReadDiscrete(address);
	}

	public bool[] ReadDiscrete(string address, ushort length)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		return dictModbusDataPool.GetModbusPool(b).ReadDiscrete(address, length);
	}

	public void WriteDiscrete(string address, bool data)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		dictModbusDataPool.GetModbusPool(b).WriteDiscrete(address, data);
	}

	public void WriteDiscrete(string address, bool[] data)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		dictModbusDataPool.GetModbusPool(b).WriteDiscrete(address, data);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		if (address.StartsWith("file=", StringComparison.OrdinalIgnoreCase))
		{
			int num = HslHelper.ExtractParameter(ref address, "file", 0);
			ushort num2 = ushort.Parse(address);
			return OperateResult.CreateSuccessResult(fileBuffer.GetBytes(num2 * 2, length * 2));
		}
		return dictModbusDataPool.GetModbusPool(b).Read(address, length);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		if (address.StartsWith("file=", StringComparison.OrdinalIgnoreCase))
		{
			int num = HslHelper.ExtractParameter(ref address, "file", 0);
			ushort num2 = ushort.Parse(address);
			fileBuffer.SetBytes(value, num2 * 2);
			return OperateResult.CreateSuccessResult();
		}
		return dictModbusDataPool.GetModbusPool(b).Write(address, value);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		return dictModbusDataPool.GetModbusPool(b).ReadBool(address, length);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		byte b = (byte)HslHelper.ExtractParameter(ref address, "s", station);
		return dictModbusDataPool.GetModbusPool(b).Write(address, value);
	}

	public void Write(string address, byte high, byte low)
	{
		Write(address, new byte[2] { high, low });
	}

	protected override INetMessage GetNewNetMessage()
	{
		return UseModbusRtuOverTcp ? null : new ModbusTcpMessage();
	}

	protected override OperateResult<byte[]> ReadFromCoreServer(PipeSession session, byte[] receive)
	{
		if (receive.Length < 3)
		{
			return new OperateResult<byte[]>("Uknown Data：" + receive.ToHexString(' '));
		}
		if (RequestDelayTime > 0)
		{
			HslHelper.ThreadSleep(RequestDelayTime);
		}
		if (session.Communication is PipeSerialPort)
		{
			if (receive[0] == 58 && receive[1] >= 48 && receive[1] < 128)
			{
				OperateResult<byte[]> operateResult = ModbusInfo.TransAsciiPackCommandToCore(receive);
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
				byte[] content = operateResult.Content;
				if (!CheckModbusMessageLegal(content))
				{
					return new OperateResult<byte[]>("Unlegal Data：" + receive.ToHexString(' '));
				}
				if (content[0] != 0 && content[0] != byte.MaxValue && !StationDataIsolation && StationCheck && station != content[0])
				{
					return new OperateResult<byte[]>("Station not match Modbus-Ascii : " + SoftBasic.GetAsciiStringRender(receive));
				}
				byte[] value = ModbusInfo.TransModbusCoreToAsciiPackCommand(ReadFromModbusCore(content));
				if (content[0] != 0)
				{
					return OperateResult.CreateSuccessResult(value);
				}
				byte[] value2 = null;
				return OperateResult.CreateSuccessResult(value2);
			}
			if (SoftCRC16.CheckCRC16(receive))
			{
				byte[] array = receive.RemoveLast(2);
				if (!CheckModbusMessageLegal(array))
				{
					return new OperateResult<byte[]>("Unlegal Data：" + receive.ToHexString(' '));
				}
				if (array[0] != 0 && array[0] != byte.MaxValue && !StationDataIsolation && StationCheck && station != array[0])
				{
					return new OperateResult<byte[]>("Station not match Modbus-rtu : " + receive.ToHexString(' '));
				}
				byte[] value3 = ModbusInfo.PackCommandToRtu(ReadFromModbusCore(array));
				if (array[0] != 0)
				{
					return OperateResult.CreateSuccessResult(value3);
				}
				byte[] value4 = null;
				return OperateResult.CreateSuccessResult(value4);
			}
			return new OperateResult<byte[]>("CRC Check Failed : " + receive.ToHexString(' '));
		}
		if (UseModbusRtuOverTcp)
		{
			if (receive[0] == 58 && receive[1] >= 48 && receive[1] < 128)
			{
				OperateResult<byte[]> operateResult2 = ModbusInfo.TransAsciiPackCommandToCore(receive);
				if (!operateResult2.IsSuccess)
				{
					return new OperateResult<byte[]>("ASCII Check Failed: " + operateResult2.Message + " Source: " + receive.ToHexString(' '));
				}
				if (!CheckModbusMessageLegal(operateResult2.Content))
				{
					return new OperateResult<byte[]>("Modbus Ascii message check failed ");
				}
				if (operateResult2.Content[0] != 0 && !StationDataIsolation && StationCheck && station != operateResult2.Content[0])
				{
					return new OperateResult<byte[]>($"Station not match Modbus-ascii, Need {station} actual {operateResult2.Content[0]}");
				}
				byte[] value5 = ModbusInfo.TransModbusCoreToAsciiPackCommand(ReadFromModbusCore(operateResult2.Content));
				if (operateResult2.Content[0] != 0)
				{
					return OperateResult.CreateSuccessResult(value5);
				}
				byte[] value6 = null;
				return OperateResult.CreateSuccessResult(value6);
			}
			if (!SoftCRC16.CheckCRC16(receive))
			{
				return new OperateResult<byte[]>("CRC Check Failed: " + receive.ToHexString(' '));
			}
			byte[] array2 = receive.RemoveLast(2);
			if (!CheckModbusMessageLegal(array2))
			{
				return new OperateResult<byte[]>("Modbus rtu message check failed ");
			}
			if (array2[0] != 0 && array2[0] != byte.MaxValue && !StationDataIsolation && StationCheck && station != array2[0])
			{
				return new OperateResult<byte[]>($"Station not match Modbus-rtu, Need {station} actual {array2[0]}");
			}
			byte[] value7 = ModbusInfo.PackCommandToRtu(ReadFromModbusCore(array2));
			if (array2[0] != 0)
			{
				return OperateResult.CreateSuccessResult(value7);
			}
			byte[] value8 = null;
			return OperateResult.CreateSuccessResult(value8);
		}
		if (!CheckModbusMessageLegal(receive.RemoveBegin(6)))
		{
			return new OperateResult<byte[]>("Modbus message check failed");
		}
		ushort id = (ushort)(receive[0] * 256 + receive[1]);
		if (receive[6] != 0 && receive[6] != byte.MaxValue && !StationDataIsolation && StationCheck && station != receive[6])
		{
			return new OperateResult<byte[]>("Station not match Modbus-tcp ");
		}
		byte[] value9 = ModbusInfo.PackCommandToTcp(ReadFromModbusCore(receive.RemoveBegin(6)), id);
		if (receive[6] == 0)
		{
			byte[] value10 = null;
			return OperateResult.CreateSuccessResult(value10);
		}
		return OperateResult.CreateSuccessResult(value9);
	}

	private byte[] CreateExceptionBack(byte[] modbusCore, byte error)
	{
		return new byte[3]
		{
			modbusCore[0],
			(byte)(modbusCore[1] + 128),
			error
		};
	}

	private byte[] CreateReadBack(byte[] modbusCore, byte[] content)
	{
		return SoftBasic.SpliceArray<byte>(new byte[3]
		{
			modbusCore[0],
			modbusCore[1],
			(byte)content.Length
		}, content);
	}

	private byte[] CreateWriteBack(byte[] modbus)
	{
		return modbus.SelectBegin(6);
	}

	private byte[] ReadCoilBack(byte[] modbus, string addressHead)
	{
		try
		{
			ushort num = byteTransformSelf.TransUInt16(modbus, 2);
			ushort num2 = byteTransformSelf.TransUInt16(modbus, 4);
			if (num + num2 > 65536)
			{
				return CreateExceptionBack(modbus, 2);
			}
			if (num2 > 2040)
			{
				return CreateExceptionBack(modbus, 3);
			}
			bool[] content = dictModbusDataPool.GetModbusPool(modbus[0]).ReadBool(addressHead + num, num2).Content;
			return CreateReadBack(modbus, SoftBasic.BoolArrayToByte(content));
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.ModbusTcpReadCoilException, ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] ReadRegisterBack(byte[] modbus, string addressHead)
	{
		try
		{
			ushort num = byteTransformSelf.TransUInt16(modbus, 2);
			ushort num2 = byteTransformSelf.TransUInt16(modbus, 4);
			if (num + num2 > 65536)
			{
				return CreateExceptionBack(modbus, 2);
			}
			if (num2 > 127)
			{
				return CreateExceptionBack(modbus, 3);
			}
			byte[] content = dictModbusDataPool.GetModbusPool(modbus[0]).Read(addressHead + num, num2).Content;
			return CreateReadBack(modbus, content);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.ModbusTcpReadRegisterException, ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] ReadFileRecordBack(byte[] modbus)
	{
		try
		{
			int num = (modbus.Length - 3) / 7;
			MemoryStream memoryStream = new MemoryStream();
			for (int i = 0; i < num; i++)
			{
				ushort num2 = byteTransformSelf.TransUInt16(modbus, 4 + 7 * i);
				ushort num3 = byteTransformSelf.TransUInt16(modbus, 6 + 7 * i);
				ushort num4 = byteTransformSelf.TransUInt16(modbus, 8 + 7 * i);
				if (num4 * 2 + 1 > 255)
				{
					return CreateExceptionBack(modbus, 3);
				}
				memoryStream.WriteByte((byte)(num4 * 2 + 1));
				memoryStream.WriteByte(6);
				memoryStream.Write(fileBuffer.GetBytes(num3 * 2, num4 * 2));
				if (memoryStream.Length > 255)
				{
					return CreateExceptionBack(modbus, 3);
				}
			}
			return CreateReadBack(modbus, memoryStream.ToArray());
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), "ReadFileRecordBack", ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] WriteFileRecordBack(byte[] modbus)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			ushort num3;
			for (int i = 3; i < modbus.Length; i += 7 + num3 * 2)
			{
				ushort num = byteTransformSelf.TransUInt16(modbus, i + 1);
				ushort num2 = byteTransformSelf.TransUInt16(modbus, i + 3);
				num3 = byteTransformSelf.TransUInt16(modbus, i + 5);
				OperateResult operateResult = Write($"file={num};{num2}", modbus.SelectMiddle(i + 7, num3 * 2));
				if (!operateResult.IsSuccess)
				{
					return CreateExceptionBack(modbus, 4);
				}
				memoryStream.Write(modbus.SelectMiddle(i, 7));
			}
			return CreateReadBack(modbus, memoryStream.ToArray());
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), "WriteFileRecordBack", ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] ReadWriteRegisterBack(byte[] modbus, string addressHead)
	{
		try
		{
			byte[] array = new byte[6]
			{
				modbus[0],
				3,
				modbus[2],
				modbus[3],
				modbus[4],
				modbus[5]
			};
			byte[] array2 = ReadRegisterBack(array, addressHead);
			if (array2[1] > 128)
			{
				return array;
			}
			byte[] array3 = SoftBasic.SpliceArray<byte>(new byte[2]
			{
				modbus[0],
				16
			}, modbus.RemoveBegin(6));
			byte[] array4 = WriteRegisterBack(array3);
			if (array4[1] > 128)
			{
				return array3;
			}
			array2[1] = modbus[1];
			return array2;
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.ModbusTcpFunctionCodeReadWriteException, ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] WriteOneCoilBack(byte[] modbus)
	{
		try
		{
			if (!base.EnableWrite)
			{
				return CreateExceptionBack(modbus, 4);
			}
			ushort num = byteTransformSelf.TransUInt16(modbus, 2);
			if (modbus[4] == byte.MaxValue && modbus[5] == 0)
			{
				dictModbusDataPool.GetModbusPool(modbus[0]).Write(num.ToString(), new bool[1] { true });
			}
			else if (modbus[4] == 0 && modbus[5] == 0)
			{
				dictModbusDataPool.GetModbusPool(modbus[0]).Write(num.ToString(), new bool[1]);
			}
			return CreateWriteBack(modbus);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.ModbusTcpWriteCoilException, ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] WriteOneRegisterBack(byte[] modbus)
	{
		try
		{
			if (!base.EnableWrite)
			{
				return CreateExceptionBack(modbus, 4);
			}
			ushort address = byteTransformSelf.TransUInt16(modbus, 2);
			short content = ReadInt16(address.ToString()).Content;
			dictModbusDataPool.GetModbusPool(modbus[0]).Write(address.ToString(), new byte[2]
			{
				modbus[4],
				modbus[5]
			});
			short content2 = ReadInt16(address.ToString()).Content;
			OnRegisterBeforWrite(address, content, content2);
			return CreateWriteBack(modbus);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.ModbusTcpWriteRegisterException, ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] WriteCoilsBack(byte[] modbus)
	{
		try
		{
			if (!base.EnableWrite)
			{
				return CreateExceptionBack(modbus, 4);
			}
			ushort num = byteTransformSelf.TransUInt16(modbus, 2);
			ushort num2 = byteTransformSelf.TransUInt16(modbus, 4);
			if (num + num2 > 65536)
			{
				return CreateExceptionBack(modbus, 2);
			}
			if (num2 > 2040)
			{
				return CreateExceptionBack(modbus, 3);
			}
			dictModbusDataPool.GetModbusPool(modbus[0]).Write(num.ToString(), modbus.RemoveBegin(7).ToBoolArray(num2));
			return CreateWriteBack(modbus);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.ModbusTcpWriteCoilException, ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] WriteRegisterBack(byte[] modbus)
	{
		try
		{
			if (!base.EnableWrite)
			{
				return CreateExceptionBack(modbus, 4);
			}
			ushort num = byteTransformSelf.TransUInt16(modbus, 2);
			ushort num2 = byteTransformSelf.TransUInt16(modbus, 4);
			if (num + num2 > 65536)
			{
				return CreateExceptionBack(modbus, 2);
			}
			if (num2 > 127)
			{
				return CreateExceptionBack(modbus, 3);
			}
			byte[] content = dictModbusDataPool.GetModbusPool(modbus[0]).Read(num.ToString(), num2).Content;
			dictModbusDataPool.GetModbusPool(modbus[0]).Write(num.ToString(), modbus.RemoveBegin(7));
			MonitorAddress[] array = new MonitorAddress[num2];
			for (ushort num3 = 0; num3 < num2; num3++)
			{
				short valueOrigin = base.ByteTransform.TransInt16(content, num3 * 2);
				short valueNew = base.ByteTransform.TransInt16(modbus, num3 * 2 + 7);
				array[num3] = new MonitorAddress
				{
					Address = (ushort)(num + num3),
					ValueOrigin = valueOrigin,
					ValueNew = valueNew
				};
			}
			for (int i = 0; i < array.Length; i++)
			{
				OnRegisterBeforWrite(array[i].Address, array[i].ValueOrigin, array[i].ValueNew);
			}
			return CreateWriteBack(modbus);
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.ModbusTcpWriteRegisterException, ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	private byte[] WriteMaskRegisterBack(byte[] modbus)
	{
		try
		{
			if (!base.EnableWrite)
			{
				return CreateExceptionBack(modbus, 4);
			}
			if (!EnableWriteMaskCode)
			{
				return CreateExceptionBack(modbus, 1);
			}
			ushort address = byteTransformSelf.TransUInt16(modbus, 2);
			int num = base.ByteTransform.TransUInt16(modbus, 4);
			int num2 = base.ByteTransform.TransUInt16(modbus, 6);
			int content = ReadInt16($"s={modbus[0]};" + address).Content;
			short num3 = (short)((content & num) | num2);
			Write($"s={modbus[0]};" + address, num3);
			MonitorAddress monitorAddress = new MonitorAddress
			{
				Address = address,
				ValueOrigin = (short)content,
				ValueNew = num3
			};
			OnRegisterBeforWrite(monitorAddress.Address, monitorAddress.ValueOrigin, monitorAddress.ValueNew);
			return modbus;
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), StringResources.Language.ModbusTcpWriteRegisterException, ex);
			return CreateExceptionBack(modbus, 4);
		}
	}

	public void AddSubcription(ModBusMonitorAddress monitor)
	{
		subcriptionHybirdLock.Enter();
		subscriptions.Add(monitor);
		subcriptionHybirdLock.Leave();
	}

	public void RemoveSubcrption(ModBusMonitorAddress monitor)
	{
		subcriptionHybirdLock.Enter();
		subscriptions.Remove(monitor);
		subcriptionHybirdLock.Leave();
	}

	private void OnRegisterBeforWrite(ushort address, short before, short after)
	{
		subcriptionHybirdLock.Enter();
		for (int i = 0; i < subscriptions.Count; i++)
		{
			if (subscriptions[i].Address == address)
			{
				subscriptions[i].SetValue(after);
				if (before != after)
				{
					subscriptions[i].SetChangeValue(before, after);
				}
			}
		}
		subcriptionHybirdLock.Leave();
	}

	private bool CheckModbusMessageLegal(byte[] buffer)
	{
		bool flag = false;
		switch (buffer[1])
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
			flag = buffer.Length == 6;
			break;
		case 15:
		case 16:
			flag = buffer.Length > 6 && buffer[6] == buffer.Length - 7;
			break;
		case 22:
			flag = buffer.Length == 8;
			break;
		case 20:
			flag = buffer[2] + 3 == buffer.Length;
			break;
		case 21:
			flag = buffer[2] + 3 == buffer.Length;
			break;
		default:
			flag = true;
			break;
		}
		if (!flag)
		{
			base.LogNet?.WriteError(ToString(), "Receive Nosense Modbus Msg : " + buffer.ToHexString(' '));
		}
		return flag;
	}

	protected virtual byte[] ReadFromModbusCore(byte[] modbusCore)
	{
		byte b = modbusCore[1];
		if (1 == 0)
		{
		}
		byte[] result = b switch
		{
			1 => ReadCoilBack(modbusCore, string.Empty), 
			2 => ReadCoilBack(modbusCore, "x=2;"), 
			3 => ReadRegisterBack(modbusCore, string.Empty), 
			4 => ReadRegisterBack(modbusCore, "x=4;"), 
			5 => WriteOneCoilBack(modbusCore), 
			6 => WriteOneRegisterBack(modbusCore), 
			15 => WriteCoilsBack(modbusCore), 
			16 => WriteRegisterBack(modbusCore), 
			22 => WriteMaskRegisterBack(modbusCore), 
			23 => ReadWriteRegisterBack(modbusCore, string.Empty), 
			20 => ReadFileRecordBack(modbusCore), 
			21 => WriteFileRecordBack(modbusCore), 
			_ => CreateExceptionBack(modbusCore, 1), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	protected override bool CheckSerialReceiveDataComplete(byte[] buffer, int dataLength)
	{
		if (dataLength > 5)
		{
			if (ModbusInfo.CheckAsciiReceiveDataComplete(buffer, dataLength))
			{
				return true;
			}
			if (ModbusInfo.CheckServerRtuReceiveDataComplete(buffer.SelectBegin(dataLength)))
			{
				return true;
			}
		}
		return false;
	}

	protected override string GetLogTextFromBinary(PipeSession session, byte[] content)
	{
		if (session.Communication is PipeSerialPort)
		{
			if (content[0] == 58 && content[1] >= 48 && content[1] < 128)
			{
				return "[Ascii] " + SoftBasic.GetAsciiStringRender(content);
			}
			return "[Rtu] " + content.ToHexString(' ');
		}
		if (session.Communication is PipeTcpNet && content != null && content.Length > 2 && content[0] == 58 && content[1] >= 48 && content[1] < 128)
		{
			return "[Ascii] " + SoftBasic.GetAsciiStringRender(content);
		}
		return base.GetLogTextFromBinary(session, content);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			subcriptionHybirdLock?.Dispose();
			subscriptions?.Clear();
			dictModbusDataPool?.Dispose();
			GC.Collect();
		}
		base.Dispose(disposing);
	}

	[HslMqttApi("ReadInt32Array", "")]
	public override OperateResult<int[]> ReadInt32(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, (ushort)(length * base.WordLength * 2)), (byte[] m) => transform.TransInt32(m, 0, length));
	}

	[HslMqttApi("ReadUInt32Array", "")]
	public override OperateResult<uint[]> ReadUInt32(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, (ushort)(length * base.WordLength * 2)), (byte[] m) => transform.TransUInt32(m, 0, length));
	}

	[HslMqttApi("ReadFloatArray", "")]
	public override OperateResult<float[]> ReadFloat(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, (ushort)(length * base.WordLength * 2)), (byte[] m) => transform.TransSingle(m, 0, length));
	}

	[HslMqttApi("ReadInt64Array", "")]
	public override OperateResult<long[]> ReadInt64(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, (ushort)(length * base.WordLength * 4)), (byte[] m) => transform.TransInt64(m, 0, length));
	}

	[HslMqttApi("ReadUInt64Array", "")]
	public override OperateResult<ulong[]> ReadUInt64(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, (ushort)(length * base.WordLength * 4)), (byte[] m) => transform.TransUInt64(m, 0, length));
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(Read(address, (ushort)(length * base.WordLength * 4)), (byte[] m) => transform.TransDouble(m, 0, length));
	}

	[HslMqttApi("WriteInt32Array", "")]
	public override OperateResult Write(string address, int[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteUInt32Array", "")]
	public override OperateResult Write(string address, uint[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteFloatArray", "")]
	public override OperateResult Write(string address, float[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteInt64Array", "")]
	public override OperateResult Write(string address, long[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteUInt64Array", "")]
	public override OperateResult Write(string address, ulong[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	[HslMqttApi("WriteDoubleArray", "")]
	public override OperateResult Write(string address, double[] values)
	{
		IByteTransform byteTransform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return Write(address, byteTransform.TransByte(values));
	}

	public override async Task<OperateResult<int[]>> ReadInt32Async(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, (ushort)(length * base.WordLength * 2)), (byte[] m) => transform.TransInt32(m, 0, length));
	}

	public override async Task<OperateResult<uint[]>> ReadUInt32Async(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, (ushort)(length * base.WordLength * 2)), (byte[] m) => transform.TransUInt32(m, 0, length));
	}

	public override async Task<OperateResult<float[]>> ReadFloatAsync(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, (ushort)(length * base.WordLength * 2)), (byte[] m) => transform.TransSingle(m, 0, length));
	}

	public override async Task<OperateResult<long[]>> ReadInt64Async(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, (ushort)(length * base.WordLength * 4)), (byte[] m) => transform.TransInt64(m, 0, length));
	}

	public override async Task<OperateResult<ulong[]>> ReadUInt64Async(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, (ushort)(length * base.WordLength * 4)), (byte[] m) => transform.TransUInt64(m, 0, length));
	}

	public override async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		IByteTransform transform = HslHelper.ExtractTransformParameter(ref address, base.ByteTransform);
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, (ushort)(length * base.WordLength * 4)), (byte[] m) => transform.TransDouble(m, 0, length));
	}

	public override async Task<OperateResult> WriteAsync(string address, int[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, uint[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, float[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, long[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, ulong[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await WriteAsync(value: HslHelper.ExtractTransformParameter(ref address, base.ByteTransform).TransByte(values), address: address);
	}

	public override string ToString()
	{
		return $"ModbusTcpServer[{base.Port}]";
	}
}
