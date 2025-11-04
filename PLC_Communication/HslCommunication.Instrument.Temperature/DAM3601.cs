using System.Linq;
using HslCommunication.ModBus;

namespace HslCommunication.Instrument.Temperature;

public class DAM3601 : ModbusRtu
{
	public DAM3601()
	{
		base.SleepTime = 200;
	}

	public DAM3601(byte station)
		: base(station)
	{
		base.SleepTime = 200;
	}

	public OperateResult<float[]> ReadAllTemperature()
	{
		string address = "x=4;1";
		if (base.AddressStartWithZero)
		{
			address = "x=4;0";
		}
		OperateResult<short[]> operateResult = ReadInt16(address, 128);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<float[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.Select((short m) => TransformValue(m)).ToArray());
	}

	private float TransformValue(short value)
	{
		if ((value & 0x800) > 0)
		{
			return (float)(((value & 0xFFF) ^ 0xFFF) + 1) * -0.0625f;
		}
		return (float)(value & 0x7FF) * 0.0625f;
	}

	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte[][]> operateResult = ModbusInfo.BuildReadModbusCommand(address, length, base.Station, base.AddressStartWithZero, 16);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<byte[]>();
		}
		return ReadFromCoreServer(operateResult.Content[0]);
	}

	public override string ToString()
	{
		return $"DAM3601[{base.PortName}:{base.BaudRate}]";
	}
}
