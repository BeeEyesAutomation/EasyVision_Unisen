using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;
using Newtonsoft.Json;

namespace HslCommunication.Robot.EFORT;

public class ER7BC10Previous : NetworkDoubleBase, IRobotNet
{
	private SoftIncrementCount softIncrementCount;

	public ER7BC10Previous(string ipAddress, int port)
	{
		IpAddress = ipAddress;
		Port = port;
		base.ByteTransform = new RegularByteTransform();
		softIncrementCount = new SoftIncrementCount(65535L, 0L);
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new EFORTMessagePrevious();
	}

	public byte[] GetReadCommand()
	{
		byte[] array = new byte[36];
		Encoding.ASCII.GetBytes("MessageHead").CopyTo(array, 0);
		BitConverter.GetBytes((ushort)array.Length).CopyTo(array, 15);
		BitConverter.GetBytes((ushort)1001).CopyTo(array, 17);
		BitConverter.GetBytes((ushort)softIncrementCount.GetCurrentValue()).CopyTo(array, 19);
		Encoding.ASCII.GetBytes("MessageTail").CopyTo(array, 21);
		return array;
	}

	[HslMqttApi(ApiTopic = "ReadRobotByte", Description = "Read the robot's original byte data information according to the address")]
	public OperateResult<byte[]> Read(string address)
	{
		return ReadFromCoreServer(GetReadCommand());
	}

	[HslMqttApi(ApiTopic = "ReadRobotString", Description = "Read the string data information of the robot based on the address")]
	public OperateResult<string> ReadString(string address)
	{
		OperateResult<EfortData> operateResult = ReadEfortData();
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(JsonConvert.SerializeObject(operateResult.Content, Formatting.Indented));
	}

	[HslMqttApi(ApiTopic = "WriteRobotByte", Description = "This robot does not support this method operation, will always return failed, invalid operation")]
	public OperateResult Write(string address, byte[] value)
	{
		return new OperateResult(StringResources.Language.NotSupportedFunction);
	}

	[HslMqttApi(ApiTopic = "WriteRobotString", Description = "This robot does not support this method operation, will always return failed, invalid operation")]
	public OperateResult Write(string address, string value)
	{
		return new OperateResult(StringResources.Language.NotSupportedFunction);
	}

	[HslMqttApi(Description = "Read the details of the robot")]
	public OperateResult<EfortData> ReadEfortData()
	{
		OperateResult<byte[]> operateResult = Read("");
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<EfortData>(operateResult);
		}
		return EfortData.PraseFromPrevious(operateResult.Content);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string address)
	{
		return await ReadFromCoreServerAsync(GetReadCommand());
	}

	public async Task<OperateResult<string>> ReadStringAsync(string address)
	{
		OperateResult<EfortData> read = await ReadEfortDataAsync();
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		return OperateResult.CreateSuccessResult(JsonConvert.SerializeObject(read.Content, Formatting.Indented));
	}

	public async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return new OperateResult(StringResources.Language.NotSupportedFunction);
	}

	public async Task<OperateResult> WriteAsync(string address, string value)
	{
		return new OperateResult(StringResources.Language.NotSupportedFunction);
	}

	public async Task<OperateResult<EfortData>> ReadEfortDataAsync()
	{
		OperateResult<byte[]> read = await ReadAsync("");
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<EfortData>(read);
		}
		return EfortData.PraseFromPrevious(read.Content);
	}

	public override string ToString()
	{
		return $"ER7BC10 Pre Robot[{IpAddress}:{Port}]";
	}
}
