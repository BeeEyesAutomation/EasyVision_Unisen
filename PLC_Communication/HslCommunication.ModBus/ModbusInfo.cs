using System;
using System.Collections.Generic;
using System.IO;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Address;
using HslCommunication.Serial;

namespace HslCommunication.ModBus;

public class ModbusInfo
{
	public const byte ReadCoil = 1;

	public const byte ReadDiscrete = 2;

	public const byte ReadRegister = 3;

	public const byte ReadInputRegister = 4;

	public const byte WriteOneCoil = 5;

	public const byte WriteOneRegister = 6;

	public const byte WriteCoil = 15;

	public const byte WriteRegister = 16;

	public const byte ReadFileRecord = 20;

	public const byte WriteFileRecord = 21;

	public const byte WriteMaskRegister = 22;

	public const byte ReadWrite = 23;

	public const byte FunctionCodeNotSupport = 1;

	public const byte FunctionCodeOverBound = 2;

	public const byte FunctionCodeQuantityOver = 3;

	public const byte FunctionCodeReadWriteException = 4;

	private static void CheckModbusAddressStart(ModbusAddress mAddress, bool isStartWithZero)
	{
		if (!isStartWithZero)
		{
			if (mAddress.AddressStart < 1)
			{
				throw new Exception(StringResources.Language.ModbusAddressMustMoreThanOne);
			}
			mAddress.AddressStart -= 1;
		}
	}

	public static OperateResult<byte[][]> BuildReadModbusCommand(string address, int length, byte station, bool isStartWithZero, byte defaultFunction)
	{
		try
		{
			ModbusAddress mAddress = new ModbusAddress(address, station, defaultFunction);
			CheckModbusAddressStart(mAddress, isStartWithZero);
			return BuildReadModbusCommand(mAddress, length, 120);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[][]>(ex.Message);
		}
	}

	public static OperateResult<byte[][]> BuildReadModbusCommand(IModbus modbus, string address, int length, byte defaultFunction)
	{
		try
		{
			ModbusAddress mAddress = new ModbusAddress(address, modbus.Station, defaultFunction);
			CheckModbusAddressStart(mAddress, modbus.AddressStartWithZero);
			return BuildReadModbusCommand(mAddress, length, modbus.WordReadBatchLength);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[][]>(ex.Message);
		}
	}

	internal static OperateResult<byte[][]> BuildReadFileModbusCommand(ushort fileNumber, ushort address, ushort length, byte station, bool addressStartWithZero)
	{
		OperateResult<int[], int[]> operateResult = HslHelper.SplitReadLength(address, length, Authorization.asdniasnfaksndiqwhawfskhfaiw() ? 124 : int.MaxValue);
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < operateResult.Content1.Length; i++)
		{
			byte[] array = new byte[10];
			array[0] = station;
			array[1] = 20;
			array[2] = (byte)(array.Length - 3);
			array[3] = 6;
			array[4] = BitConverter.GetBytes(fileNumber)[1];
			array[5] = BitConverter.GetBytes(fileNumber)[0];
			array[6] = BitConverter.GetBytes(operateResult.Content1[i])[1];
			array[7] = BitConverter.GetBytes(operateResult.Content1[i])[0];
			array[8] = BitConverter.GetBytes(operateResult.Content2[i])[1];
			array[9] = BitConverter.GetBytes(operateResult.Content2[i])[0];
			list.Add(array);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	internal static OperateResult<byte[]> BuildWriteFileModbusCommand(ushort fileNumber, ushort address, byte[] data, byte station, bool addressStartWithZero)
	{
		if (data.Length + 7 > 255)
		{
			return new OperateResult<byte[]>("the data length must less than 248 bytes( 124 word)");
		}
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.WriteByte(station);
		memoryStream.WriteByte(21);
		memoryStream.WriteByte((byte)(7 + data.Length));
		memoryStream.WriteByte(6);
		memoryStream.WriteByte(BitConverter.GetBytes(fileNumber)[1]);
		memoryStream.WriteByte(BitConverter.GetBytes(fileNumber)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(address)[1]);
		memoryStream.WriteByte(BitConverter.GetBytes(address)[0]);
		memoryStream.WriteByte(BitConverter.GetBytes(data.Length / 2)[1]);
		memoryStream.WriteByte(BitConverter.GetBytes(data.Length / 2)[0]);
		memoryStream.Write(data);
		return OperateResult.CreateSuccessResult(memoryStream.ToArray());
	}

	public static OperateResult<byte[]> BuildReadWriteModbusCommand(string readAddress, ushort length, string writeAddress, byte[] value, byte station, bool isStartWithZero, byte defaultFunction)
	{
		try
		{
			ModbusAddress modbusAddress = new ModbusAddress(readAddress, station, defaultFunction);
			CheckModbusAddressStart(modbusAddress, isStartWithZero);
			ModbusAddress modbusAddress2 = new ModbusAddress(writeAddress, station, defaultFunction);
			CheckModbusAddressStart(modbusAddress2, isStartWithZero);
			byte[] array = new byte[11 + value.Length];
			array[0] = (byte)modbusAddress.Station;
			array[1] = (byte)modbusAddress.Function;
			array[2] = BitConverter.GetBytes(modbusAddress.AddressStart)[1];
			array[3] = BitConverter.GetBytes(modbusAddress.AddressStart)[0];
			array[4] = BitConverter.GetBytes(length)[1];
			array[5] = BitConverter.GetBytes(length)[0];
			array[6] = BitConverter.GetBytes(modbusAddress2.AddressStart)[1];
			array[7] = BitConverter.GetBytes(modbusAddress2.AddressStart)[0];
			array[8] = (byte)(value.Length / 2 / 256);
			array[9] = (byte)(value.Length / 2 % 256);
			array[10] = (byte)value.Length;
			value.CopyTo(array, 11);
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[][]> BuildReadModbusCommand(ModbusAddress mAddress, int length, int cuttingBatchLength)
	{
		List<byte[]> list = new List<byte[]>();
		if (mAddress.Function == 1 || mAddress.Function == 2 || mAddress.Function == 3 || mAddress.Function == 4 || Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			OperateResult<int[], int[]> operateResult = HslHelper.SplitReadLength(mAddress.AddressStart, length, (mAddress.Function == 1 || mAddress.Function == 2) ? 2000 : cuttingBatchLength);
			for (int i = 0; i < operateResult.Content1.Length; i++)
			{
				list.Add(new byte[6]
				{
					(byte)mAddress.Station,
					(byte)mAddress.Function,
					BitConverter.GetBytes(operateResult.Content1[i])[1],
					BitConverter.GetBytes(operateResult.Content1[i])[0],
					BitConverter.GetBytes(operateResult.Content2[i])[1],
					BitConverter.GetBytes(operateResult.Content2[i])[0]
				});
			}
		}
		else
		{
			list.Add(new byte[6]
			{
				(byte)mAddress.Station,
				(byte)mAddress.Function,
				BitConverter.GetBytes(mAddress.AddressStart)[1],
				BitConverter.GetBytes(mAddress.AddressStart)[0],
				BitConverter.GetBytes(length)[1],
				BitConverter.GetBytes(length)[0]
			});
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public static OperateResult<byte[]> BuildWriteBoolModbusCommand(string address, bool[] values, byte station, bool isStartWithZero, byte defaultFunction)
	{
		try
		{
			ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
			CheckModbusAddressStart(modbusAddress, isStartWithZero);
			if (modbusAddress.Function == 1)
			{
				modbusAddress.Function = defaultFunction;
			}
			return BuildWriteBoolModbusCommand(modbusAddress, values);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteBoolModbusCommand(string address, bool value, byte station, bool isStartWithZero, byte defaultFunction)
	{
		try
		{
			if (address.IndexOf('.') <= 0)
			{
				ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
				CheckModbusAddressStart(modbusAddress, isStartWithZero);
				if (modbusAddress.Function == 1)
				{
					modbusAddress.Function = defaultFunction;
				}
				return BuildWriteBoolModbusCommand(modbusAddress, value);
			}
			if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
			{
				return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
			}
			int num = Convert.ToInt32(address.Substring(address.IndexOf('.') + 1));
			if (num < 0 || num > 15)
			{
				return new OperateResult<byte[]>(StringResources.Language.ModbusBitIndexOverstep);
			}
			int num2 = 1 << num;
			int num3 = ~num2;
			if (!value)
			{
				num2 = 0;
			}
			return BuildWriteMaskModbusCommand(address.Substring(0, address.IndexOf('.')), (ushort)num3, (ushort)num2, station, isStartWithZero, 22);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteBoolModbusCommand(ModbusAddress mAddress, bool[] values)
	{
		try
		{
			byte[] array = SoftBasic.BoolArrayToByte(values);
			byte[] array2 = new byte[7 + array.Length];
			array2[0] = (byte)mAddress.Station;
			if (mAddress.WriteFunction < 0)
			{
				array2[1] = (byte)mAddress.Function;
			}
			else
			{
				array2[1] = (byte)mAddress.WriteFunction;
			}
			array2[2] = BitConverter.GetBytes(mAddress.AddressStart)[1];
			array2[3] = BitConverter.GetBytes(mAddress.AddressStart)[0];
			array2[4] = (byte)(values.Length / 256);
			array2[5] = (byte)(values.Length % 256);
			array2[6] = (byte)array.Length;
			array.CopyTo(array2, 7);
			return OperateResult.CreateSuccessResult(array2);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteBoolModbusCommand(ModbusAddress mAddress, bool value)
	{
		byte[] array = new byte[6]
		{
			(byte)mAddress.Station,
			0,
			0,
			0,
			0,
			0
		};
		if (mAddress.WriteFunction < 0)
		{
			array[1] = (byte)mAddress.Function;
		}
		else
		{
			array[1] = (byte)mAddress.WriteFunction;
		}
		array[2] = BitConverter.GetBytes(mAddress.AddressStart)[1];
		array[3] = BitConverter.GetBytes(mAddress.AddressStart)[0];
		if (value)
		{
			array[4] = byte.MaxValue;
			array[5] = 0;
		}
		else
		{
			array[4] = 0;
			array[5] = 0;
		}
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteWordModbusCommand(string address, byte[] values, byte station, bool isStartWithZero, byte defaultFunction)
	{
		try
		{
			ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
			if (modbusAddress.Function == 3)
			{
				modbusAddress.Function = defaultFunction;
			}
			CheckModbusAddressStart(modbusAddress, isStartWithZero);
			return BuildWriteWordModbusCommand(modbusAddress, values);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteWordModbusCommand(string address, short value, byte station, bool isStartWithZero, byte defaultFunction, IByteTransform byteTransform)
	{
		try
		{
			ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
			if (modbusAddress.Function == 3)
			{
				modbusAddress.Function = defaultFunction;
			}
			if (modbusAddress.WriteFunction == 16 || modbusAddress.Function == 16)
			{
				CheckModbusAddressStart(modbusAddress, isStartWithZero);
				byte[] values = byteTransform.TransByte(value);
				return BuildWriteWordModbusCommand(modbusAddress, values);
			}
			CheckModbusAddressStart(modbusAddress, isStartWithZero);
			return BuildWriteOneRegisterModbusCommand(modbusAddress, value, byteTransform);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteWordModbusCommand(string address, ushort value, byte station, bool isStartWithZero, byte defaultFunction, IByteTransform byteTransform)
	{
		try
		{
			ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
			if (modbusAddress.Function == 3)
			{
				modbusAddress.Function = defaultFunction;
			}
			if (modbusAddress.WriteFunction == 16 || modbusAddress.Function == 16)
			{
				CheckModbusAddressStart(modbusAddress, isStartWithZero);
				byte[] values = byteTransform.TransByte(value);
				return BuildWriteWordModbusCommand(modbusAddress, values);
			}
			CheckModbusAddressStart(modbusAddress, isStartWithZero);
			return BuildWriteOneRegisterModbusCommand(modbusAddress, value, byteTransform);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteMaskModbusCommand(string address, ushort andMask, ushort orMask, byte station, bool isStartWithZero, byte defaultFunction)
	{
		try
		{
			ModbusAddress modbusAddress = new ModbusAddress(address, station, defaultFunction);
			if (modbusAddress.Function == 3)
			{
				modbusAddress.Function = defaultFunction;
			}
			CheckModbusAddressStart(modbusAddress, isStartWithZero);
			return BuildWriteMaskModbusCommand(modbusAddress, andMask, orMask);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static OperateResult<byte[]> BuildWriteWordModbusCommand(ModbusAddress mAddress, byte[] values)
	{
		byte[] array = new byte[7 + values.Length];
		array[0] = (byte)mAddress.Station;
		if (mAddress.WriteFunction < 0)
		{
			array[1] = (byte)mAddress.Function;
		}
		else
		{
			array[1] = (byte)mAddress.WriteFunction;
		}
		array[2] = BitConverter.GetBytes(mAddress.AddressStart)[1];
		array[3] = BitConverter.GetBytes(mAddress.AddressStart)[0];
		array[4] = (byte)(values.Length / 2 / 256);
		array[5] = (byte)(values.Length / 2 % 256);
		array[6] = (byte)values.Length;
		values.CopyTo(array, 7);
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteMaskModbusCommand(ModbusAddress mAddress, ushort andMask, ushort orMask)
	{
		return OperateResult.CreateSuccessResult(new byte[8]
		{
			(byte)mAddress.Station,
			(byte)mAddress.Function,
			BitConverter.GetBytes(mAddress.AddressStart)[1],
			BitConverter.GetBytes(mAddress.AddressStart)[0],
			BitConverter.GetBytes(andMask)[1],
			BitConverter.GetBytes(andMask)[0],
			BitConverter.GetBytes(orMask)[1],
			BitConverter.GetBytes(orMask)[0]
		});
	}

	public static OperateResult<byte[]> BuildWriteOneRegisterModbusCommand(ModbusAddress mAddress, short value, IByteTransform byteTransform)
	{
		byte[] array = new byte[6]
		{
			(byte)mAddress.Station,
			0,
			0,
			0,
			0,
			0
		};
		if (mAddress.WriteFunction < 0)
		{
			array[1] = (byte)mAddress.Function;
		}
		else
		{
			array[1] = (byte)mAddress.WriteFunction;
		}
		array[2] = BitConverter.GetBytes(mAddress.AddressStart)[1];
		array[3] = BitConverter.GetBytes(mAddress.AddressStart)[0];
		array[4] = byteTransform.TransByte(value)[0];
		array[5] = byteTransform.TransByte(value)[1];
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> BuildWriteOneRegisterModbusCommand(ModbusAddress mAddress, ushort value, IByteTransform byteTransform)
	{
		byte[] array = new byte[6]
		{
			(byte)mAddress.Station,
			0,
			0,
			0,
			0,
			0
		};
		if (mAddress.WriteFunction < 0)
		{
			array[1] = (byte)mAddress.Function;
		}
		else
		{
			array[1] = (byte)mAddress.WriteFunction;
		}
		array[2] = BitConverter.GetBytes(mAddress.AddressStart)[1];
		array[3] = BitConverter.GetBytes(mAddress.AddressStart)[0];
		array[4] = byteTransform.TransByte(value)[0];
		array[5] = byteTransform.TransByte(value)[1];
		return OperateResult.CreateSuccessResult(array);
	}

	public static OperateResult<byte[]> ExtractActualData(byte[] response)
	{
		try
		{
			if (response[1] >= 128)
			{
				return new OperateResult<byte[]>(response[2], GetDescriptionByErrorCode(response[2]));
			}
			if (response.Length > 3)
			{
				return OperateResult.CreateSuccessResult(SoftBasic.ArrayRemoveBegin(response, 3));
			}
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>(ex.Message);
		}
	}

	public static byte[] PackCommandToTcp(byte[] modbus, ushort id)
	{
		byte[] array = new byte[modbus.Length + 6];
		array[0] = BitConverter.GetBytes(id)[1];
		array[1] = BitConverter.GetBytes(id)[0];
		array[4] = BitConverter.GetBytes(modbus.Length)[1];
		array[5] = BitConverter.GetBytes(modbus.Length)[0];
		modbus.CopyTo(array, 6);
		return array;
	}

	public static byte[] ExplodeTcpCommandToCore(byte[] modbusTcp)
	{
		return modbusTcp.RemoveBegin(6);
	}

	public static byte[] ExplodeRtuCommandToCore(byte[] modbusRtu)
	{
		return modbusRtu.RemoveLast(2);
	}

	public static byte[] PackCommandToRtu(byte[] modbus)
	{
		return SoftCRC16.CRC16(modbus);
	}

	public static byte[] TransModbusCoreToAsciiPackCommand(byte[] modbus)
	{
		byte[] inBytes = SoftLRC.LRC(modbus);
		byte[] array = SoftBasic.BytesToAsciiBytes(inBytes);
		return SoftBasic.SpliceArray<byte>(new byte[1] { 58 }, array, new byte[2] { 13, 10 });
	}

	public static OperateResult<byte[]> TransAsciiPackCommandToCore(byte[] modbusAscii)
	{
		try
		{
			if (modbusAscii[0] != 58 || modbusAscii[modbusAscii.Length - 2] != 13 || modbusAscii[modbusAscii.Length - 1] != 10)
			{
				return new OperateResult<byte[]>
				{
					Message = StringResources.Language.ModbusAsciiFormatCheckFailed + modbusAscii.ToHexString(' ')
				};
			}
			int num = -1;
			for (int i = 1; i < modbusAscii.Length - 1; i++)
			{
				if (modbusAscii[i] == 13 && modbusAscii[i + 1] == 10)
				{
					num = i;
					break;
				}
			}
			if (num > 1 && num != modbusAscii.Length - 2)
			{
				modbusAscii = modbusAscii.SelectBegin(num + 2);
			}
			byte[] array = SoftBasic.AsciiBytesToBytes(modbusAscii.RemoveDouble(1, 2));
			if (!SoftLRC.CheckLRC(array))
			{
				return new OperateResult<byte[]>
				{
					Message = StringResources.Language.ModbusLRCCheckFailed + array.ToHexString(' ')
				};
			}
			return OperateResult.CreateSuccessResult(array.RemoveLast(1));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>
			{
				Message = ex.Message + modbusAscii.ToHexString(' ')
			};
		}
	}

	public static OperateResult<ModbusAddress> AnalysisAddress(string address, byte defaultStation, bool isStartWithZero, byte defaultFunction)
	{
		try
		{
			ModbusAddress modbusAddress = new ModbusAddress(address, defaultStation, defaultFunction);
			if (!isStartWithZero)
			{
				if (modbusAddress.AddressStart < 1)
				{
					throw new Exception(StringResources.Language.ModbusAddressMustMoreThanOne);
				}
				modbusAddress.AddressStart -= 1;
			}
			return OperateResult.CreateSuccessResult(modbusAddress);
		}
		catch (Exception ex)
		{
			return new OperateResult<ModbusAddress>
			{
				Message = ex.Message
			};
		}
	}

	public static string GetDescriptionByErrorCode(byte code)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			1 => StringResources.Language.ModbusTcpFunctionCodeNotSupport, 
			2 => StringResources.Language.ModbusTcpFunctionCodeOverBound, 
			3 => StringResources.Language.ModbusTcpFunctionCodeQuantityOver, 
			4 => StringResources.Language.ModbusTcpFunctionCodeReadWriteException, 
			_ => StringResources.Language.UnknownError, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static bool CheckRtuReceiveDataComplete(byte[] response, int startIndex = 0, bool checkCrc = true)
	{
		if (response.Length > 3 + startIndex)
		{
			if (response[startIndex + 1] == 6 || response[startIndex + 1] == 16 || response[startIndex + 1] == 15 || response[startIndex + 1] == 5)
			{
				if (response.Length >= 8 + startIndex)
				{
					if (checkCrc)
					{
						return SoftCRC16.CheckCRC16(response.SelectMiddle(startIndex, 8));
					}
					return true;
				}
			}
			else if (response[startIndex + 1] == 1 || response[startIndex + 1] == 2 || response[startIndex + 1] == 3 || response[startIndex + 1] == 4 || response[startIndex + 1] == 23 || response[startIndex + 1] == 20 || response[startIndex + 1] == 21)
			{
				if (response.Length >= response[2 + startIndex] + 3 + 2 + startIndex)
				{
					if (checkCrc)
					{
						return SoftCRC16.CheckCRC16(response.SelectMiddle(startIndex, response[2 + startIndex] + 3 + 2));
					}
					return true;
				}
			}
			else if (response[startIndex + 1] == 22)
			{
				if (response.Length >= 10 + startIndex)
				{
					if (checkCrc)
					{
						return SoftCRC16.CheckCRC16(response.SelectMiddle(startIndex, 10));
					}
					return true;
				}
			}
			else
			{
				if (response[startIndex + 1] <= 128)
				{
					if (checkCrc)
					{
						return SoftCRC16.CheckCRC16(response.RemoveBegin(startIndex));
					}
					return true;
				}
				if (response.Length >= 5 + startIndex)
				{
					if (checkCrc)
					{
						return SoftCRC16.CheckCRC16(response.SelectMiddle(startIndex, 5));
					}
					return true;
				}
			}
		}
		return false;
	}

	public static bool CheckRtuReceiveDataComplete(byte[] send, byte[] response)
	{
		if (send == null)
		{
			return CheckRtuReceiveDataComplete(response);
		}
		if (send.Length < 5)
		{
			return CheckRtuReceiveDataComplete(response);
		}
		if (response == null)
		{
			return false;
		}
		int num = Math.Min(response.Length - 3, 3);
		for (int i = 0; i < num; i++)
		{
			if (send[0] == response[i] && CheckRtuReceiveDataComplete(response, i))
			{
				return true;
			}
		}
		for (int j = 0; j < num; j++)
		{
			if (send[0] == response[j] && CheckRtuReceiveDataComplete(response, j, checkCrc: false))
			{
				return true;
			}
		}
		return false;
	}

	public static int CheckRtuMessageMatch(byte[] send, byte[] receive)
	{
		if (send == null)
		{
			return 1;
		}
		if (send.Length < 5)
		{
			return 1;
		}
		if (receive == null)
		{
			return 1;
		}
		for (int i = 0; i < receive.Length - 3; i++)
		{
			if (send[0] == receive[i] && CheckRtuReceiveDataComplete(receive, i, checkCrc: false))
			{
				return 1;
			}
		}
		return -1;
	}

	public static bool CheckServerRtuReceiveDataComplete(byte[] receive)
	{
		if (receive.Length > 2)
		{
			if (receive[1] == 16 || receive[1] == 15)
			{
				return receive.Length > 8 && receive.Length >= receive[6] + 7 + 2;
			}
			if (receive[1] == 1 || receive[1] == 2 || receive[1] == 3 || receive[1] == 4 || receive[1] == 6 || receive[1] == 5)
			{
				return receive.Length >= 8;
			}
			if (receive[1] == 22)
			{
				return receive.Length >= 10;
			}
			if (receive[1] == 23)
			{
				return receive.Length >= 11 + receive[10] + 2;
			}
			if (receive[1] == 20 || receive[1] == 21)
			{
				return receive.Length >= 3 + receive[2] + 2;
			}
		}
		return false;
	}

	public static bool CheckAsciiReceiveDataComplete(byte[] modbusAscii)
	{
		return CheckAsciiReceiveDataComplete(modbusAscii, modbusAscii.Length);
	}

	public static bool CheckAsciiReceiveDataComplete(byte[] modbusAscii, int length)
	{
		if (length > 5)
		{
			return modbusAscii[0] == 58 && modbusAscii[length - 2] == 13 && modbusAscii[length - 1] == 10;
		}
		return false;
	}
}
