using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;

namespace HslCommunication.Robot.KUKA;

public class KukaAvarProxyNet : NetworkDoubleBase, IRobotNet
{
	private SoftIncrementCount softIncrementCount;

	public KukaAvarProxyNet()
	{
		softIncrementCount = new SoftIncrementCount(65535L, 0L);
		base.ByteTransform = new RegularByteTransform(DataFormat.CDAB);
	}

	public KukaAvarProxyNet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new KukaVarProxyMessage();
	}

	[HslMqttApi(ApiTopic = "ReadRobotByte", Description = "Read the data content of the Kuka robot according to the input variable name")]
	public OperateResult<byte[]> Read(string address)
	{
		return ByteTransformHelper.GetResultFromOther(ReadFromCoreServer(PackCommand(BuildReadValueCommand(address))), ExtractActualData);
	}

	[HslMqttApi(ApiTopic = "ReadRobotString", Description = "Read all the data information of the Kuka robot, return the string information, decode by ANSI, need to specify the variable name")]
	public OperateResult<string> ReadString(string address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(Read(address), Encoding.Default.GetString);
	}

	[HslMqttApi(ApiTopic = "WriteRobotByte", Description = "Write the original data content according to the variable name of the Kuka robot")]
	public OperateResult Write(string address, byte[] value)
	{
		return Write(address, Encoding.Default.GetString(value));
	}

	[HslMqttApi(ApiTopic = "WriteRobotString", Description = "Writes ansi-encoded string data information based on the variable name of the Kuka robot")]
	public OperateResult Write(string address, string value)
	{
		return ByteTransformHelper.GetResultFromOther(ReadFromCoreServer(PackCommand(BuildWriteValueCommand(address, value))), ExtractActualData);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string address)
	{
		return ByteTransformHelper.GetResultFromOther(await ReadFromCoreServerAsync(PackCommand(BuildReadValueCommand(address))), ExtractActualData);
	}

	public async Task<OperateResult<string>> ReadStringAsync(string address)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(await ReadAsync(address), Encoding.Default.GetString);
	}

	public async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await WriteAsync(address, Encoding.Default.GetString(value));
	}

	public async Task<OperateResult> WriteAsync(string address, string value)
	{
		return ByteTransformHelper.GetResultFromOther(await ReadFromCoreServerAsync(PackCommand(BuildWriteValueCommand(address, value))), ExtractActualData);
	}

	private byte[] PackCommand(byte[] commandCore)
	{
		byte[] array = new byte[commandCore.Length + 4];
		base.ByteTransform.TransByte((ushort)softIncrementCount.GetCurrentValue()).CopyTo(array, 0);
		base.ByteTransform.TransByte((ushort)commandCore.Length).CopyTo(array, 2);
		commandCore.CopyTo(array, 4);
		return array;
	}

	private OperateResult<byte[]> ExtractActualData(byte[] response)
	{
		try
		{
			if (response[response.Length - 1] != 1)
			{
				return new OperateResult<byte[]>(response[response.Length - 1], "Wrong: " + SoftBasic.ByteToHexString(response, ' '));
			}
			int num = response[5] * 256 + response[6];
			byte[] array = new byte[num];
			Array.Copy(response, 7, array, 0, num);
			return OperateResult.CreateSuccessResult(array);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Wrong:" + ex.Message + " Code:" + SoftBasic.ByteToHexString(response, ' '));
		}
	}

	private byte[] BuildCommands(byte function, string[] commands)
	{
		List<byte> list = new List<byte>();
		list.Add(function);
		for (int i = 0; i < commands.Length; i++)
		{
			byte[] bytes = Encoding.Default.GetBytes(commands[i]);
			list.AddRange(base.ByteTransform.TransByte((ushort)bytes.Length));
			list.AddRange(bytes);
		}
		return list.ToArray();
	}

	private byte[] BuildReadValueCommand(string address)
	{
		return BuildCommands(0, new string[1] { address });
	}

	private byte[] BuildWriteValueCommand(string address, string value)
	{
		return BuildCommands(1, new string[2] { address, value });
	}

	public override string ToString()
	{
		return $"KukaAvarProxyNet Robot[{IpAddress}:{Port}]";
	}
}
