using System.IO.Ports;
using HslCommunication.Reflection;
using HslCommunication.Serial;

namespace HslCommunication.Instrument.Light;

public class ShineInLightSourceController : SerialBase
{
	public override void SerialPortInni(string portName, int baudRate)
	{
		SerialPortInni(portName, baudRate, 8, StopBits.One, Parity.Even);
	}

	public override void SerialPortInni(string portName)
	{
		SerialPortInni(portName, 57600);
	}

	[HslMqttApi(ApiTopic = "Read", Description = "读取光源控制器的参数信息，需要传入通道号信息，返回 ShineInLightData 对象")]
	public OperateResult<ShineInLightData> Read(byte channel)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(BuildReadCommand(channel));
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<ShineInLightData>();
		}
		OperateResult<byte[]> operateResult2 = ExtractActualData(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2.ConvertFailed<ShineInLightData>();
		}
		return OperateResult.CreateSuccessResult(new ShineInLightData(operateResult2.Content));
	}

	[HslMqttApi(ApiTopic = "Write", Description = "将光源控制器的数据写入到设备，返回是否写入成功")]
	public OperateResult Write(ShineInLightData data)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(BuildWriteCommand(data));
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<ShineInLightData>();
		}
		return ExtractActualData(operateResult.Content);
	}

	public override string ToString()
	{
		return "ShineInLightSourceController[" + base.PortName + "]";
	}

	public static byte[] PackCommand(byte cmd, byte[] data)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		byte[] array = new byte[data.Length + 8];
		array[0] = 47;
		array[1] = 42;
		array[2] = 240;
		array[3] = cmd;
		array[4] = (byte)(array.Length - 4);
		data.CopyTo(array, 5);
		array[array.Length - 2] = 42;
		array[array.Length - 1] = 47;
		int num = array[2];
		for (int i = 3; i < array.Length - 3; i++)
		{
			num ^= array[i];
		}
		array[array.Length - 3] = (byte)num;
		return array;
	}

	public static byte[] BuildWriteCommand(ShineInLightData shineInLightData)
	{
		return PackCommand(1, shineInLightData.GetSourceData());
	}

	public static byte[] BuildReadCommand(byte channel)
	{
		return PackCommand(2, new byte[1] { channel });
	}

	public static OperateResult<byte[]> ExtractActualData(byte[] response)
	{
		if (response.Length < 9)
		{
			return new OperateResult<byte[]>("Receive Data is too short; source:" + response.ToHexString(' '));
		}
		if (response[0] != 47 || response[1] != 42 || response[response.Length - 2] != 42 || response[response.Length - 1] != 47)
		{
			return new OperateResult<byte[]>("Receive Data not start with /* or end with */; source:" + response.ToHexString());
		}
		if (response[3] == 1)
		{
			return (response[5] == 170) ? OperateResult.CreateSuccessResult(new byte[0]) : new OperateResult<byte[]>(response[5], "set not success");
		}
		return OperateResult.CreateSuccessResult(response.SelectMiddle(5, response.Length - 8));
	}
}
