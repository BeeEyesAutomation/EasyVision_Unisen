using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.Pipe;
using HslCommunication.Profinet.Melsec.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Melsec;

public class MelsecFxSerialOverTcp : DeviceTcpNet, IMelsecFxSerial, IReadWriteNet
{
	private List<string> inis = new List<string>
	{
		"00 10 02 FF FF FC 01 10 03", "10025E000000FC00000000001212000000FFFF030000FF0300002D001C091A18000000000000000000FC000012120414000113960AC4E5D7010201000000023030453032303203364310033543", "10025E000000FC00000000001212000000FFFF030000FF0300002D001C091A18000000000000000000FC000012120414000113960AC4E5D7010202000000023030454341303203384510033833", "10025E000000FC00000000001212000000FFFF030000FF0300002D001C091A18000000000000000000FC000012120414000113960AC4E5D7010203000000023030453032303203364310033545", "10025E000000FC00000000001212000000FFFF030000FF0300002D001C091A18000000000000000000FC000012120414000113960AC4E5D7010204000000023030454341303203384510033835", "10025E000000FC00000000001212000000FFFF030000FF0300002F001C091A18000000000000000000FC000012120414000113960AC4E5D70102050000000245303138303030343003443510034342", "10025E000000FC00000000001212000000FFFF030000FF0300002F001C091A18000000000000000000FC000012120414000113960AC4E5D70102060000000245303138303430314303453910034535", "10025E000000FC00000000001212000000FFFF030000FF0300002F001C091A18000000000000000000FC000012120414000113960AC4E5D70102070000000245303030453030343003453110034436", "10025E000000FC00000000001212000000FFFF030000FF0300002F001C091A18000000000000000000FC000012120414000113960AC4E5D70102080000000245303030453430343003453510034446", "10025E000000FC00000000001212000000FFFF030000FF0300002F001C091A18000000000000000000FC000012120414000113960AC4E5D70102090000000245303030453830343003453910034538",
		"10025E000000FC00000000001212000000FFFF030000FF0300002F001C091A18000000000000000000FC000012120414000113960AC4E5D701020A0000000245303030454330343003463410034630"
	};

	private bool useGot = false;

	private SoftIncrementCount incrementCount;

	public bool UseGOT
	{
		get
		{
			return useGot;
		}
		set
		{
			useGot = value;
		}
	}

	public bool IsNewVersion { get; set; }

	public MelsecFxSerialOverTcp()
	{
		base.WordLength = 1;
		base.ByteTransform = new RegularByteTransform();
		IsNewVersion = true;
		base.ByteTransform.IsStringReverseByteWord = true;
		base.SleepTime = 20;
		incrementCount = new SoftIncrementCount(2147483647L, 1L);
		LogMsgFormatBinary = false;
	}

	public MelsecFxSerialOverTcp(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	private byte[] GetBytesSend(byte[] command)
	{
		List<byte> list = new List<byte>();
		for (int i = 0; i < command.Length; i++)
		{
			if (i < 2)
			{
				list.Add(command[i]);
			}
			else if (i < command.Length - 4)
			{
				if (command[i] == 16)
				{
					list.Add(command[i]);
				}
				list.Add(command[i]);
			}
			else
			{
				list.Add(command[i]);
			}
		}
		return list.ToArray();
	}

	private byte[] GetBytesReceive(byte[] response)
	{
		List<byte> list = new List<byte>();
		for (int i = 0; i < response.Length; i++)
		{
			if (i < 2)
			{
				list.Add(response[i]);
			}
			else if (i < response.Length - 4)
			{
				if (response[i] == 16 && response[i + 1] == 16)
				{
					list.Add(response[i]);
					i++;
				}
				else
				{
					list.Add(response[i]);
				}
			}
			else
			{
				list.Add(response[i]);
			}
		}
		return list.ToArray();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		if (useGot)
		{
			byte[] array = new byte[66 + command.Length];
			array[0] = 16;
			array[1] = 2;
			array[2] = 94;
			array[6] = 252;
			array[12] = 18;
			array[13] = 18;
			array[17] = byte.MaxValue;
			array[18] = byte.MaxValue;
			array[19] = 3;
			array[22] = byte.MaxValue;
			array[23] = 3;
			array[26] = BitConverter.GetBytes(34 + command.Length)[0];
			array[27] = BitConverter.GetBytes(34 + command.Length)[1];
			array[28] = 28;
			array[29] = 9;
			array[30] = 26;
			array[31] = 24;
			array[41] = 252;
			array[44] = 18;
			array[45] = 18;
			array[46] = 4;
			array[47] = 20;
			array[49] = 1;
			array[50] = BitConverter.GetBytes(Port)[1];
			array[51] = BitConverter.GetBytes(Port)[0];
			array[52] = IPAddress.Parse(IpAddress).GetAddressBytes()[0];
			array[53] = IPAddress.Parse(IpAddress).GetAddressBytes()[1];
			array[54] = IPAddress.Parse(IpAddress).GetAddressBytes()[2];
			array[55] = IPAddress.Parse(IpAddress).GetAddressBytes()[3];
			array[56] = 1;
			array[57] = 2;
			BitConverter.GetBytes((int)incrementCount.GetCurrentValue()).CopyTo(array, 58);
			command.CopyTo(array, 62);
			array[array.Length - 4] = 16;
			array[array.Length - 3] = 3;
			MelsecHelper.FxCalculateCRC(array, 2, 4).CopyTo(array, array.Length - 2);
			return GetBytesSend(array);
		}
		return base.PackCommandWithHeader(command);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		if (useGot)
		{
			if (response.Length > 68)
			{
				response = GetBytesReceive(response);
				int num = -1;
				for (int i = 0; i < response.Length - 4; i++)
				{
					if (response[i] == 16 && response[i + 1] == 2)
					{
						num = i;
						break;
					}
				}
				if (num >= 0)
				{
					return OperateResult.CreateSuccessResult(response.RemoveDouble(64 + num, 4));
				}
			}
			return new OperateResult<byte[]>("Got failed: " + response.ToHexString(' ', 16));
		}
		return base.UnpackResponseContent(send, response);
	}

	protected override OperateResult InitializationOnConnect()
	{
		if (useGot)
		{
			for (int i = 0; i < inis.Count; i++)
			{
				OperateResult operateResult = ReadFromCoreServer(CommunicationPipe, inis[i].ToHexBytes(), hasResponseData: true, usePackAndUnpack: false);
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
			}
		}
		return base.InitializationOnConnect();
	}

	public override OperateResult<byte[]> ReadFromCoreServer(CommunicationPipe pipe, byte[] send, bool hasResponseData = true, bool usePackAndUnpack = true)
	{
		OperateResult<byte[]> operateResult = base.ReadFromCoreServer(pipe, send, hasResponseData, usePackAndUnpack);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (operateResult.Content == null)
		{
			return operateResult;
		}
		if (operateResult.Content.Length > 2)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = base.ReadFromCoreServer(pipe, send, hasResponseData, usePackAndUnpack);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<byte>(operateResult.Content, operateResult2.Content));
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(CommunicationPipe pipe, byte[] send, bool hasResponseData = true, bool usePackAndUnpack = true)
	{
		OperateResult<byte[]> read = await base.ReadFromCoreServerAsync(pipe, send, hasResponseData, usePackAndUnpack).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		if (read.Content == null)
		{
			return read;
		}
		if (read.Content.Length > 2)
		{
			return read;
		}
		OperateResult<byte[]> read2 = await base.ReadFromCoreServerAsync(pipe, send, hasResponseData, usePackAndUnpack).ConfigureAwait(continueOnCapturedContext: false);
		if (!read2.IsSuccess)
		{
			return read2;
		}
		return OperateResult.CreateSuccessResult(SoftBasic.SpliceArray<byte>(read.Content, read2.Content));
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		if (useGot)
		{
			for (int i = 0; i < inis.Count; i++)
			{
				OperateResult ini1 = await ReadFromCoreServerAsync(CommunicationPipe, inis[i].ToHexBytes(), hasResponseData: true, usePackAndUnpack: false).ConfigureAwait(continueOnCapturedContext: false);
				if (!ini1.IsSuccess)
				{
					return ini1;
				}
			}
		}
		return await base.InitializationOnConnectAsync();
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return MelsecFxSerialHelper.Read(this, address, length, IsNewVersion);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return MelsecFxSerialHelper.Write(this, address, value, IsNewVersion);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await MelsecFxSerialHelper.ReadAsync(this, address, length, IsNewVersion);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await MelsecFxSerialHelper.WriteAsync(this, address, value, IsNewVersion);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return MelsecFxSerialHelper.ReadBool(this, address, length, IsNewVersion);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return MelsecFxSerialHelper.Write(this, address, value);
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return MelsecFxSerialHelper.Write(this, address, value, IsNewVersion);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await MelsecFxSerialHelper.ReadBoolAsync(this, address, length, IsNewVersion);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await MelsecFxSerialHelper.WriteAsync(this, address, value);
	}

	[HslMqttApi]
	public OperateResult ActivePlc()
	{
		return MelsecFxSerialHelper.ActivePlc(this);
	}

	public async Task<OperateResult> ActivePlcAsync()
	{
		return await MelsecFxSerialHelper.ActivePlcAsync(this);
	}

	public override string ToString()
	{
		return $"MelsecFxSerialOverTcp[{IpAddress}:{Port}]";
	}
}
