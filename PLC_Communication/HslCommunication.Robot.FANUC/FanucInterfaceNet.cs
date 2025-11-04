using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Reflection;
using Newtonsoft.Json;

namespace HslCommunication.Robot.FANUC;

public class FanucInterfaceNet : DeviceTcpNet, IRobotNet, IReadWriteNet
{
	private FanucData fanucDataRetain = null;

	private DateTime fanucDataRefreshTime = DateTime.Now.AddSeconds(-10.0);

	private PropertyInfo[] fanucDataPropertyInfo = typeof(FanucData).GetProperties();

	private byte[] connect_req = new byte[56];

	private byte[] session_req = new byte[56]
	{
		8, 0, 1, 0, 0, 0, 0, 0, 0, 1,
		0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		1, 192, 0, 0, 0, 0, 16, 14, 0, 0,
		1, 1, 79, 1, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0
	};

	public int ClientId { get; private set; } = 1024;

	public int FanucDataRetainTime { get; set; } = 100;

	public Encoding StringEncoding { get; set; }

	public FanucInterfaceNet()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		StringEncoding = Encoding.Default;
	}

	public FanucInterfaceNet(string ipAddress, int port = 60008)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new FanucRobotMessage();
	}

	private OperateResult ReadCommandFromRobot(CommunicationPipe pipe, string[] cmds)
	{
		for (int i = 0; i < cmds.Length; i++)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(cmds[i]);
			OperateResult<byte[]> operateResult = ReadFromCoreServer(pipe, FanucHelper.BuildWriteData(56, 1, bytes, bytes.Length), hasResponseData: true, usePackAndUnpack: true);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override OperateResult InitializationOnConnect()
	{
		BitConverter.GetBytes(ClientId).CopyTo(connect_req, 1);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, connect_req, hasResponseData: true, usePackAndUnpack: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		operateResult = ReadFromCoreServer(CommunicationPipe, session_req, hasResponseData: true, usePackAndUnpack: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadCommandFromRobot(CommunicationPipe, FanucHelper.GetFanucCmds());
	}

	private async Task<OperateResult> ReadCommandFromRobotAsync(CommunicationPipe pipe, string[] cmds)
	{
		for (int i = 0; i < cmds.Length; i++)
		{
			byte[] buffer = Encoding.ASCII.GetBytes(cmds[i]);
			OperateResult<byte[]> write = await ReadFromCoreServerAsync(pipe, FanucHelper.BuildWriteData(56, 1, buffer, buffer.Length), hasResponseData: true, usePackAndUnpack: true);
			if (!write.IsSuccess)
			{
				return write;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		BitConverter.GetBytes(ClientId).CopyTo(connect_req, 1);
		OperateResult<byte[]> receive2 = await ReadFromCoreServerAsync(CommunicationPipe, connect_req, hasResponseData: true, usePackAndUnpack: true).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive2.IsSuccess)
		{
			return receive2;
		}
		receive2 = await ReadFromCoreServerAsync(CommunicationPipe, session_req, hasResponseData: true, usePackAndUnpack: true).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive2.IsSuccess)
		{
			return receive2;
		}
		return await ReadCommandFromRobotAsync(CommunicationPipe, FanucHelper.GetFanucCmds()).ConfigureAwait(continueOnCapturedContext: false);
	}

	[HslMqttApi(ApiTopic = "ReadRobotByte", Description = "Read the robot's original byte data information according to the address")]
	public OperateResult<byte[]> Read(string address)
	{
		return Read(8, 1, 6130);
	}

	[HslMqttApi(ApiTopic = "ReadRobotString", Description = "Read the string data information of the robot based on the address")]
	public OperateResult<string> ReadString(string address)
	{
		if (string.IsNullOrEmpty(address))
		{
			OperateResult<FanucData> operateResult = ReadFanucData();
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult);
			}
			fanucDataRetain = operateResult.Content;
			fanucDataRefreshTime = DateTime.Now;
			return OperateResult.CreateSuccessResult(JsonConvert.SerializeObject(operateResult.Content, Formatting.Indented));
		}
		if ((DateTime.Now - fanucDataRefreshTime).TotalMilliseconds > (double)FanucDataRetainTime || fanucDataRetain == null)
		{
			OperateResult<FanucData> operateResult2 = ReadFanucData();
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult2);
			}
			fanucDataRetain = operateResult2.Content;
			fanucDataRefreshTime = DateTime.Now;
		}
		PropertyInfo[] array = fanucDataPropertyInfo;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo propertyInfo in array2)
		{
			if (propertyInfo.Name == address)
			{
				return OperateResult.CreateSuccessResult(JsonConvert.SerializeObject(propertyInfo.GetValue(fanucDataRetain, null), Formatting.Indented));
			}
		}
		return new OperateResult<string>(StringResources.Language.NotSupportedDataType);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string address)
	{
		return await ReadAsync(8, 1, 6130);
	}

	public async Task<OperateResult<string>> ReadStringAsync(string address)
	{
		if (string.IsNullOrEmpty(address))
		{
			OperateResult<FanucData> read2 = await ReadFanucDataAsync();
			if (!read2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(read2);
			}
			fanucDataRetain = read2.Content;
			fanucDataRefreshTime = DateTime.Now;
			return OperateResult.CreateSuccessResult(JsonConvert.SerializeObject(read2.Content, Formatting.Indented));
		}
		if ((DateTime.Now - fanucDataRefreshTime).TotalMilliseconds > (double)FanucDataRetainTime || fanucDataRetain == null)
		{
			OperateResult<FanucData> read3 = await ReadFanucDataAsync();
			if (!read3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(read3);
			}
			fanucDataRetain = read3.Content;
			fanucDataRefreshTime = DateTime.Now;
		}
		PropertyInfo[] array = fanucDataPropertyInfo;
		PropertyInfo[] array2 = array;
		foreach (PropertyInfo item in array2)
		{
			if (item.Name == address)
			{
				return OperateResult.CreateSuccessResult(JsonConvert.SerializeObject(item.GetValue(fanucDataRetain, null), Formatting.Indented));
			}
		}
		return new OperateResult<string>(StringResources.Language.NotSupportedDataType);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<byte, ushort> operateResult = FanucHelper.AnalysisFanucAddress(address, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (operateResult.Content1 == 8 || operateResult.Content1 == 10 || operateResult.Content1 == 12)
		{
			return Read(operateResult.Content1, operateResult.Content2, length);
		}
		return new OperateResult<byte[]>(StringResources.Language.NotSupportedDataType + ", Current address not support word read/write");
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<byte, ushort> operateResult = FanucHelper.AnalysisFanucAddress(address, isBit: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (operateResult.Content1 == 8 || operateResult.Content1 == 10 || operateResult.Content1 == 12)
		{
			return Write(operateResult.Content1, operateResult.Content2, value);
		}
		return new OperateResult<byte[]>(StringResources.Language.NotSupportedDataType + ", Current address not support word read/write");
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		OperateResult<byte, ushort> operateResult = FanucHelper.AnalysisFanucAddress(address, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		if (operateResult.Content1 == 70 || operateResult.Content1 == 72 || operateResult.Content1 == 76)
		{
			return ReadBool(operateResult.Content1, operateResult.Content2, length);
		}
		return new OperateResult<bool[]>(StringResources.Language.NotSupportedDataType + ", Current address not support bool read/write");
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		OperateResult<byte, ushort> operateResult = FanucHelper.AnalysisFanucAddress(address, isBit: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content1 == 70 || operateResult.Content1 == 72 || operateResult.Content1 == 76)
		{
			return WriteBool(operateResult.Content1, operateResult.Content2, value);
		}
		return new OperateResult(StringResources.Language.NotSupportedDataType + ", Current address not support bool read/write");
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<byte, ushort> analysis = FanucHelper.AnalysisFanucAddress(address, isBit: false);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(analysis);
		}
		if (analysis.Content1 == 8 || analysis.Content1 == 10 || analysis.Content1 == 12)
		{
			return await ReadAsync(analysis.Content1, analysis.Content2, length);
		}
		return new OperateResult<byte[]>(StringResources.Language.NotSupportedDataType + ", Current address not support word read/write");
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<byte, ushort> analysis = FanucHelper.AnalysisFanucAddress(address, isBit: false);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(analysis);
		}
		if (analysis.Content1 == 8 || analysis.Content1 == 10 || analysis.Content1 == 12)
		{
			return await WriteAsync(analysis.Content1, analysis.Content2, value);
		}
		return new OperateResult<byte[]>(StringResources.Language.NotSupportedDataType + ", Current address not support word read/write");
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		OperateResult<byte, ushort> analysis = FanucHelper.AnalysisFanucAddress(address, isBit: true);
		if (!analysis.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(analysis);
		}
		if (analysis.Content1 == 70 || analysis.Content1 == 72 || analysis.Content1 == 76)
		{
			return await ReadBoolAsync(analysis.Content1, analysis.Content2, length);
		}
		return new OperateResult<bool[]>(StringResources.Language.NotSupportedDataType + ", Current address not support bool read/write");
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		OperateResult<byte, ushort> analysis = FanucHelper.AnalysisFanucAddress(address, isBit: true);
		if (!analysis.IsSuccess)
		{
			return analysis;
		}
		if (analysis.Content1 == 70 || analysis.Content1 == 72 || analysis.Content1 == 76)
		{
			return await WriteBoolAsync(analysis.Content1, analysis.Content2, value);
		}
		return new OperateResult(StringResources.Language.NotSupportedDataType + ", Current address not support bool read/write");
	}

	public OperateResult<byte[]> Read(byte select, ushort address, ushort length)
	{
		byte[] send = FanucHelper.BulidReadData(select, address, length);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content[31] == 148)
		{
			return OperateResult.CreateSuccessResult(SoftBasic.ArrayRemoveBegin(operateResult.Content, 56));
		}
		if (operateResult.Content[31] == 212)
		{
			return OperateResult.CreateSuccessResult(SoftBasic.ArraySelectMiddle(operateResult.Content, 44, length * 2));
		}
		return new OperateResult<byte[]>(operateResult.Content[31], "Error");
	}

	public OperateResult Write(byte select, ushort address, byte[] value)
	{
		byte[] send = FanucHelper.BuildWriteData(select, address, value, value.Length / 2);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content[31] == 212)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult<byte[]>(operateResult.Content[31], "Error");
	}

	public OperateResult<bool[]> ReadBool(byte select, ushort address, ushort length)
	{
		int num = address - 1 - (address - 1) % 8 + 1;
		int num2 = (((address + length - 1) % 8 == 0) ? (address + length - 1) : ((address + length - 1) / 8 * 8 + 8));
		int num3 = (num2 - num + 1) / 8;
		byte[] send = FanucHelper.BulidReadData(select, address, (ushort)(num3 * 8));
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		if (operateResult.Content[31] == 148)
		{
			bool[] sourceArray = SoftBasic.ByteToBoolArray(SoftBasic.ArrayRemoveBegin(operateResult.Content, 56));
			bool[] array = new bool[length];
			Array.Copy(sourceArray, address - num, array, 0, length);
			return OperateResult.CreateSuccessResult(array);
		}
		if (operateResult.Content[31] == 212)
		{
			bool[] sourceArray2 = SoftBasic.ByteToBoolArray(SoftBasic.ArraySelectMiddle(operateResult.Content, 44, num3));
			bool[] array2 = new bool[length];
			Array.Copy(sourceArray2, address - num, array2, 0, length);
			return OperateResult.CreateSuccessResult(array2);
		}
		return new OperateResult<bool[]>(operateResult.Content[31], "Error");
	}

	public OperateResult WriteBool(byte select, ushort address, bool[] value)
	{
		int num = address - 1 - (address - 1) % 8 + 1;
		int num2 = (((address + value.Length - 1) % 8 == 0) ? (address + value.Length - 1) : ((address + value.Length - 1) / 8 * 8 + 8));
		int num3 = (num2 - num + 1) / 8;
		bool[] array = new bool[num3 * 8];
		Array.Copy(value, 0, array, address - num, value.Length);
		byte[] send = FanucHelper.BuildWriteData(select, address, base.ByteTransform.TransByte(array), value.Length);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<short[]>(operateResult);
		}
		if (operateResult.Content[31] == 212)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult();
	}

	public async Task<OperateResult<byte[]>> ReadAsync(byte select, ushort address, ushort length)
	{
		byte[] send = FanucHelper.BulidReadData(select, address, length);
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(send);
		if (!read.IsSuccess)
		{
			return read;
		}
		if (read.Content[31] == 148)
		{
			return OperateResult.CreateSuccessResult(SoftBasic.ArrayRemoveBegin(read.Content, 56));
		}
		if (read.Content[31] == 212)
		{
			return OperateResult.CreateSuccessResult(SoftBasic.ArraySelectMiddle(read.Content, 44, length * 2));
		}
		return new OperateResult<byte[]>(read.Content[31], "Error");
	}

	public async Task<OperateResult> WriteAsync(byte select, ushort address, byte[] value)
	{
		byte[] send = FanucHelper.BuildWriteData(select, address, value, value.Length / 2);
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(send);
		if (!read.IsSuccess)
		{
			return read;
		}
		if (read.Content[31] == 212)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult<byte[]>(read.Content[31], "Error");
	}

	public async Task<OperateResult<bool[]>> ReadBoolAsync(byte select, ushort address, ushort length)
	{
		int byteStartIndex = address - 1 - (address - 1) % 8 + 1;
		int byteEndIndex = (((address + length - 1) % 8 == 0) ? (address + length - 1) : ((address + length - 1) / 8 * 8 + 8));
		int byteLength = (byteEndIndex - byteStartIndex + 1) / 8;
		byte[] send = FanucHelper.BulidReadData(select, address, (ushort)(byteLength * 8));
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(send);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		if (read.Content[31] == 148)
		{
			bool[] array2 = SoftBasic.ByteToBoolArray(SoftBasic.ArrayRemoveBegin(read.Content, 56));
			bool[] buffer2 = new bool[length];
			Array.Copy(array2, address - byteStartIndex, buffer2, 0, length);
			return OperateResult.CreateSuccessResult(buffer2);
		}
		if (read.Content[31] == 212)
		{
			bool[] array3 = SoftBasic.ByteToBoolArray(SoftBasic.ArraySelectMiddle(read.Content, 44, byteLength));
			bool[] buffer3 = new bool[length];
			Array.Copy(array3, address - byteStartIndex, buffer3, 0, length);
			return OperateResult.CreateSuccessResult(buffer3);
		}
		return new OperateResult<bool[]>(read.Content[31], "Error");
	}

	public async Task<OperateResult> WriteBoolAsync(byte select, ushort address, bool[] value)
	{
		int byteStartIndex = address - 1 - (address - 1) % 8 + 1;
		int byteEndIndex = (((address + value.Length - 1) % 8 == 0) ? (address + value.Length - 1) : ((address + value.Length - 1) / 8 * 8 + 8));
		int byteLength = (byteEndIndex - byteStartIndex + 1) / 8;
		bool[] buffer = new bool[byteLength * 8];
		Array.Copy(value, 0, buffer, address - byteStartIndex, value.Length);
		byte[] send = FanucHelper.BuildWriteData(select, address, base.ByteTransform.TransByte(buffer), value.Length);
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(send);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<short[]>(read);
		}
		if (read.Content[31] == 212)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult();
	}

	[HslMqttApi(Description = "Read the details of the robot and return the resolved data type")]
	public OperateResult<FanucData> ReadFanucData()
	{
		OperateResult<byte[]> operateResult = Read("");
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FanucData>(operateResult);
		}
		return FanucData.PraseFrom(operateResult.Content, StringEncoding);
	}

	[HslMqttApi(Description = "Read the SDO information of the robot")]
	public OperateResult<bool[]> ReadSDO(ushort address, ushort length)
	{
		return ReadBool("SDO" + address, length);
	}

	[HslMqttApi(Description = "Write the SDO information of the robot")]
	public OperateResult WriteSDO(ushort address, bool[] value)
	{
		return Write("SDO" + address, value);
	}

	[HslMqttApi(Description = "Read the SDI information of the robot")]
	public OperateResult<bool[]> ReadSDI(ushort address, ushort length)
	{
		return ReadBool("SDI" + address, length);
	}

	[HslMqttApi(Description = "Write the SDI information of the robot")]
	public OperateResult WriteSDI(ushort address, bool[] value)
	{
		return Write("SDI" + address, value);
	}

	[HslMqttApi]
	public OperateResult<bool[]> ReadRDI(ushort address, ushort length)
	{
		return ReadBool("RDI" + address, length);
	}

	[HslMqttApi]
	public OperateResult WriteRDI(ushort address, bool[] value)
	{
		return Write("RDI" + address, value);
	}

	[HslMqttApi]
	public OperateResult<bool[]> ReadUI(ushort address, ushort length)
	{
		return ReadBool("UI" + address, length);
	}

	[HslMqttApi]
	public OperateResult<bool[]> ReadUO(ushort address, ushort length)
	{
		return ReadBool("UO" + address, length);
	}

	[HslMqttApi]
	public OperateResult WriteUO(ushort address, bool[] value)
	{
		return Write("UO" + address, value);
	}

	[HslMqttApi]
	public OperateResult<bool[]> ReadSI(ushort address, ushort length)
	{
		return ReadBool("SI" + address, length);
	}

	[HslMqttApi]
	public OperateResult<bool[]> ReadSO(ushort address, ushort length)
	{
		return ReadBool("SO" + address, length);
	}

	[HslMqttApi]
	public OperateResult WriteSO(ushort address, bool[] value)
	{
		return Write("SO" + address, value);
	}

	[HslMqttApi]
	public OperateResult<ushort[]> ReadGI(ushort address, ushort length)
	{
		return ReadUInt16("GI" + address, length);
	}

	[HslMqttApi]
	public OperateResult WriteGI(ushort address, ushort[] value)
	{
		return Write("GI" + address, value);
	}

	[HslMqttApi]
	public OperateResult<ushort[]> ReadGO(ushort address, ushort length)
	{
		return ReadUInt16("GO" + address, length);
	}

	[HslMqttApi]
	public OperateResult WriteGO(ushort address, ushort[] value)
	{
		return Write("GO" + address, value);
	}

	[HslMqttApi]
	public OperateResult<bool[]> ReadPMCR2(ushort address, ushort length)
	{
		return ReadBool(76, address, length);
	}

	[HslMqttApi]
	public OperateResult WritePMCR2(ushort address, bool[] value)
	{
		return WriteBool(76, address, value);
	}

	[HslMqttApi]
	public OperateResult<bool[]> ReadRDO(ushort address, ushort length)
	{
		return ReadBool("RDO" + address, length);
	}

	[HslMqttApi]
	public OperateResult WriteRDO(ushort address, bool[] value)
	{
		return Write("RDO" + address, value);
	}

	[HslMqttApi]
	public OperateResult WriteRXyzwpr(ushort Address, float[] Xyzwpr, short[] Config, short UserFrame, short UserTool)
	{
		int num = Xyzwpr.Length * 4 + Config.Length * 2 + 2;
		byte[] array = new byte[num];
		base.ByteTransform.TransByte(Xyzwpr).CopyTo(array, 0);
		base.ByteTransform.TransByte(Config).CopyTo(array, 36);
		OperateResult operateResult = Write(8, Address, array);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (0 <= UserFrame && UserFrame <= 15)
		{
			if (0 <= UserTool && UserTool <= 15)
			{
				operateResult = Write(8, (ushort)(Address + 45), base.ByteTransform.TransByte(new short[2] { UserFrame, UserTool }));
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
			}
			else
			{
				operateResult = Write(8, (ushort)(Address + 45), base.ByteTransform.TransByte(new short[1] { UserFrame }));
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
			}
		}
		else if (0 <= UserTool && UserTool <= 15)
		{
			operateResult = Write(8, (ushort)(Address + 46), base.ByteTransform.TransByte(new short[1] { UserTool }));
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi]
	public OperateResult WriteRJoint(ushort address, float[] joint, short UserFrame, short UserTool)
	{
		OperateResult operateResult = Write(8, (ushort)(address + 26), base.ByteTransform.TransByte(joint));
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (0 <= UserFrame && UserFrame <= 15)
		{
			if (0 <= UserTool && UserTool <= 15)
			{
				operateResult = Write(8, (ushort)(address + 44), base.ByteTransform.TransByte(new short[3] { 0, UserFrame, UserTool }));
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
			}
			else
			{
				operateResult = Write(8, (ushort)(address + 44), base.ByteTransform.TransByte(new short[2] { 0, UserFrame }));
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
			}
		}
		else
		{
			operateResult = Write(8, (ushort)(address + 44), base.ByteTransform.TransByte(new short[1]));
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			if (0 <= UserTool && UserTool <= 15)
			{
				operateResult = Write(8, (ushort)(address + 44), base.ByteTransform.TransByte(new short[2] { 0, UserTool }));
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult<FanucData>> ReadFanucDataAsync()
	{
		OperateResult<byte[]> read = await ReadAsync("");
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FanucData>(read);
		}
		return FanucData.PraseFrom(read.Content, StringEncoding);
	}

	public async Task<OperateResult<bool[]>> ReadSDOAsync(ushort address, ushort length)
	{
		return await ReadBoolAsync("SDO" + address, length);
	}

	public async Task<OperateResult> WriteSDOAsync(ushort address, bool[] value)
	{
		return await WriteAsync("SDO" + address, value);
	}

	public async Task<OperateResult<bool[]>> ReadSDIAsync(ushort address, ushort length)
	{
		return await ReadBoolAsync("SDI" + address, length);
	}

	public async Task<OperateResult> WriteSDIAsync(ushort address, bool[] value)
	{
		return await WriteAsync("SDI" + address, value);
	}

	public async Task<OperateResult<bool[]>> ReadRDIAsync(ushort address, ushort length)
	{
		return await ReadBoolAsync("RDI" + address, length);
	}

	public async Task<OperateResult> WriteRDIAsync(ushort address, bool[] value)
	{
		return await WriteAsync("RDI" + address, value);
	}

	public async Task<OperateResult<bool[]>> ReadUIAsync(ushort address, ushort length)
	{
		return await ReadBoolAsync("UI" + address, length);
	}

	public async Task<OperateResult<bool[]>> ReadUOAsync(ushort address, ushort length)
	{
		return await ReadBoolAsync("UO" + address, length);
	}

	public async Task<OperateResult> WriteUOAsync(ushort address, bool[] value)
	{
		return await WriteAsync("UO" + address, value);
	}

	public async Task<OperateResult<bool[]>> ReadSIAsync(ushort address, ushort length)
	{
		return await ReadBoolAsync("SI" + address, length);
	}

	public async Task<OperateResult<bool[]>> ReadSOAsync(ushort address, ushort length)
	{
		return await ReadBoolAsync("SO" + address, length);
	}

	public async Task<OperateResult> WriteSOAsync(ushort address, bool[] value)
	{
		return await WriteAsync("SO" + address, value);
	}

	public async Task<OperateResult<ushort[]>> ReadGIAsync(ushort address, ushort length)
	{
		return await ReadUInt16Async("GI" + address, length);
	}

	public async Task<OperateResult> WriteGIAsync(ushort address, ushort[] value)
	{
		return await WriteAsync("GI" + address, value);
	}

	public async Task<OperateResult<ushort[]>> ReadGOAsync(ushort address, ushort length)
	{
		return await ReadUInt16Async("GO" + address, length);
	}

	public async Task<OperateResult> WriteGOAsync(ushort address, ushort[] value)
	{
		return await WriteAsync("GO" + address, value);
	}

	public async Task<OperateResult<bool[]>> ReadPMCR2Async(ushort address, ushort length)
	{
		return await ReadBoolAsync(76, address, length);
	}

	public async Task<OperateResult> WritePMCR2Async(ushort address, bool[] value)
	{
		return await WriteBoolAsync(76, address, value);
	}

	public async Task<OperateResult<bool[]>> ReadRDOAsync(ushort address, ushort length)
	{
		return await ReadBoolAsync("RDO" + address, length);
	}

	public async Task<OperateResult> WriteRDOAsync(ushort address, bool[] value)
	{
		return await WriteAsync("RDO" + address, value);
	}

	public async Task<OperateResult> WriteRXyzwprAsync(ushort Address, float[] Xyzwpr, short[] Config, short UserFrame, short UserTool)
	{
		int num = Xyzwpr.Length * 4 + Config.Length * 2 + 2;
		byte[] robotBuffer = new byte[num];
		base.ByteTransform.TransByte(Xyzwpr).CopyTo(robotBuffer, 0);
		base.ByteTransform.TransByte(Config).CopyTo(robotBuffer, 36);
		OperateResult write2 = await WriteAsync(8, Address, robotBuffer);
		if (!write2.IsSuccess)
		{
			return write2;
		}
		if (0 <= UserFrame && UserFrame <= 15)
		{
			if (0 <= UserTool && UserTool <= 15)
			{
				write2 = await WriteAsync(8, (ushort)(Address + 45), base.ByteTransform.TransByte(new short[2] { UserFrame, UserTool }));
				if (!write2.IsSuccess)
				{
					return write2;
				}
			}
			else
			{
				write2 = await WriteAsync(8, (ushort)(Address + 45), base.ByteTransform.TransByte(new short[1] { UserFrame }));
				if (!write2.IsSuccess)
				{
					return write2;
				}
			}
		}
		else if (0 <= UserTool && UserTool <= 15)
		{
			write2 = await WriteAsync(8, (ushort)(Address + 46), base.ByteTransform.TransByte(new short[1] { UserTool }));
			if (!write2.IsSuccess)
			{
				return write2;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult> WriteRJointAsync(ushort address, float[] joint, short UserFrame, short UserTool)
	{
		OperateResult write2 = await WriteAsync(8, (ushort)(address + 26), base.ByteTransform.TransByte(joint));
		if (!write2.IsSuccess)
		{
			return write2;
		}
		if (0 <= UserFrame && UserFrame <= 15)
		{
			if (0 <= UserTool && UserTool <= 15)
			{
				write2 = await WriteAsync(8, (ushort)(address + 44), base.ByteTransform.TransByte(new short[3] { 0, UserFrame, UserTool }));
				if (!write2.IsSuccess)
				{
					return write2;
				}
			}
			else
			{
				write2 = await WriteAsync(8, (ushort)(address + 44), base.ByteTransform.TransByte(new short[2] { 0, UserFrame }));
				if (!write2.IsSuccess)
				{
					return write2;
				}
			}
		}
		else
		{
			write2 = await WriteAsync(8, (ushort)(address + 44), base.ByteTransform.TransByte(new short[1]));
			if (!write2.IsSuccess)
			{
				return write2;
			}
			if (0 <= UserTool && UserTool <= 15)
			{
				write2 = await WriteAsync(8, (ushort)(address + 44), base.ByteTransform.TransByte(new short[2] { 0, UserTool }));
				if (!write2.IsSuccess)
				{
					return write2;
				}
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public override string ToString()
	{
		return $"FanucInterfaceNet[{IpAddress}:{Port}]";
	}
}
