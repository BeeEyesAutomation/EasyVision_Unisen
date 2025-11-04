using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Profinet.AllenBradley;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Omron;

public class OmronConnectedCipNet : NetworkConnectedCip, IReadWriteCip, IReadWriteNet
{
	public string ProductName { get; private set; }

	public byte ConnectionTimeoutMultiplier { get; set; } = 2;

	public OmronConnectedCipNet()
	{
		base.WordLength = 2;
		base.ByteTransform = new RegularByteTransform();
	}

	public OmronConnectedCipNet(string ipAddress, int port = 44818)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override byte[] GetLargeForwardOpen(ushort connectionID)
	{
		uint num = (base.TOConnectionId = (uint)(-2130837503 + connectionID));
		uint value = num;
		byte[] array = "\r\n00 00 00 00 00 00 02 00 00 00 00 00 b2 00 34 00\r\n5b 02 20 06 24 01 0e 9c 02 00 00 80 01 00 fe 80\r\n02 00 1b 05 30 a7 2b 03 02 00 00 00 80 84 1e 00\r\ncc 07 00 42 80 84 1e 00 cc 07 00 42 a3 03 20 02\r\n24 01 2c 01".ToHexBytes();
		BitConverter.GetBytes((uint)(-2147483646 + connectionID)).CopyTo(array, 24);
		BitConverter.GetBytes(value).CopyTo(array, 28);
		BitConverter.GetBytes((ushort)(2 + connectionID)).CopyTo(array, 32);
		BitConverter.GetBytes((ushort)4105).CopyTo(array, 34);
		HslHelper.HslRandom.GetBytes(4).CopyTo(array, 36);
		array[40] = ConnectionTimeoutMultiplier;
		return array;
	}

	protected override OperateResult InitializationOnConnect()
	{
		OperateResult operateResult = base.InitializationOnConnect();
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(CommunicationPipe, AllenBradleyHelper.PackRequestHeader(111, base.SessionHandle, GetAttributeAll()), hasResponseData: true, usePackAndUnpack: false);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if (operateResult2.Content.Length > 59 && operateResult2.Content.Length >= 59 + operateResult2.Content[58])
		{
			ProductName = Encoding.UTF8.GetString(operateResult2.Content, 59, operateResult2.Content[58]);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		OperateResult ini = await base.InitializationOnConnectAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (!ini.IsSuccess)
		{
			return ini;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(CommunicationPipe, AllenBradleyHelper.PackRequestHeader(111, base.SessionHandle, GetAttributeAll()), hasResponseData: true, usePackAndUnpack: false).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		if (read.Content.Length > 59 && read.Content.Length >= 59 + read.Content[58])
		{
			ProductName = Encoding.UTF8.GetString(read.Content, 59, read.Content[58]);
		}
		return OperateResult.CreateSuccessResult();
	}

	private byte[] GetAttributeAll()
	{
		return "00 00 00 00 00 00 02 00 00 00 00 00 b2 00 06 00 01 02 20 01 24 01".ToHexBytes();
	}

	private OperateResult<byte[]> BuildReadCommand(string[] address, ushort[] length)
	{
		try
		{
			List<byte[]> list = new List<byte[]>();
			for (int i = 0; i < address.Length; i++)
			{
				list.Add(AllenBradleyHelper.PackRequsetRead(address[i], length[i], isConnectedAddress: true));
			}
			return OperateResult.CreateSuccessResult(PackCommandService(list.ToArray()));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Address Wrong:" + ex.Message);
		}
	}

	private OperateResult<byte[]> BuildWriteCommand(string address, ushort typeCode, byte[] data, int length = 1)
	{
		try
		{
			return OperateResult.CreateSuccessResult(PackCommandService(AllenBradleyHelper.PackRequestWrite(address, typeCode, data, length, isConnectedAddress: true)));
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Address Wrong:" + ex.Message);
		}
	}

	private OperateResult<byte[], ushort, bool> ReadWithType(string[] address, ushort[] length)
	{
		OperateResult<byte[]> operateResult = BuildReadCommand(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(operateResult2);
		}
		OperateResult operateResult3 = AllenBradleyHelper.CheckResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(operateResult3);
		}
		return NetworkConnectedCip.ExtractActualData(operateResult2.Content, isRead: true);
	}

	public OperateResult<byte[]> ReadCipFromServer(params byte[][] cips)
	{
		byte[] send = PackCommandService(cips.ToArray());
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = AllenBradleyHelper.CheckResponse(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content);
	}

	public OperateResult<T> ReadStruct<T>(string address) where T : struct
	{
		OperateResult<byte[]> operateResult = Read(address, 1);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(operateResult);
		}
		return HslHelper.ByteArrayToStruct<T>(operateResult.Content.RemoveBegin(2));
	}

	private async Task<OperateResult<byte[], ushort, bool>> ReadWithTypeAsync(string[] address, ushort[] length)
	{
		OperateResult<byte[]> command = BuildReadCommand(address, length);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(command);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(read);
		}
		OperateResult check = AllenBradleyHelper.CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(check);
		}
		return NetworkConnectedCip.ExtractActualData(read.Content, isRead: true);
	}

	public async Task<OperateResult<byte[]>> ReadCipFromServerAsync(params byte[][] cips)
	{
		byte[] command = PackCommandService(cips.ToArray());
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = AllenBradleyHelper.CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		return OperateResult.CreateSuccessResult(read.Content);
	}

	public async Task<OperateResult<T>> ReadStructAsync<T>(string address) where T : struct
	{
		OperateResult<byte[]> read = await ReadAsync(address, 1);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(read);
		}
		return HslHelper.ByteArrayToStruct<T>(read.Content.RemoveBegin(2));
	}

	protected virtual int GetMaxTransferBytes()
	{
		return 1988;
	}

	private int GetLengthFromRemain(ushort dataType, int length)
	{
		if (dataType == 193 || dataType == 194 || dataType == 198 || dataType == 211)
		{
			return Math.Min(length, GetMaxTransferBytes());
		}
		if (dataType == 199 || dataType == 195)
		{
			return Math.Min(length, GetMaxTransferBytes() / 2);
		}
		if (dataType == 196 || dataType == 200 || dataType == 202)
		{
			return Math.Min(length, GetMaxTransferBytes() / 4);
		}
		return Math.Min(length, GetMaxTransferBytes() / 8);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		HslHelper.ExtractParameter(ref address, "type", 0);
		if (length == 1)
		{
			OperateResult<byte[], ushort, bool> operateResult = ReadWithType(new string[1] { address }, new ushort[1] { length });
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			return OperateResult.CreateSuccessResult(operateResult.Content1);
		}
		int num = 0;
		int num2 = 0;
		string format = "[{0}]";
		List<byte> list = new List<byte>();
		Match match = Regex.Match(address, "\\[[0-9]+\\]$");
		if (match.Success)
		{
			address = address.Remove(match.Index, match.Length);
			num2 = int.Parse(match.Value.Substring(1, match.Value.Length - 2));
		}
		else
		{
			Match match2 = Regex.Match(address, "\\[[0-9]+,[0-9]+\\]$");
			if (match2.Success)
			{
				address = address.Remove(match2.Index, match2.Length);
				string value = Regex.Matches(match2.Value, "[0-9]+")[1].Value;
				format = match2.Value.Replace(value + "]", "{0}]");
				num2 = int.Parse(value);
			}
		}
		ushort dataType = 0;
		while (num < length)
		{
			if (num == 0)
			{
				ushort num3 = Math.Min(length, (ushort)248);
				OperateResult<byte[], ushort, bool> operateResult2 = ReadWithType(new string[1] { address + string.Format(format, num2) }, new ushort[1] { num3 });
				if (!operateResult2.IsSuccess)
				{
					return OperateResult.CreateFailedResult<byte[]>(operateResult2);
				}
				dataType = operateResult2.Content2;
				num += num3;
				num2 += num3;
				list.AddRange(operateResult2.Content1);
			}
			else
			{
				ushort num4 = (ushort)GetLengthFromRemain(dataType, length - num);
				OperateResult<byte[], ushort, bool> operateResult3 = ReadWithType(new string[1] { address + string.Format(format, num2) }, new ushort[1] { num4 });
				if (!operateResult3.IsSuccess)
				{
					return OperateResult.CreateFailedResult<byte[]>(operateResult3);
				}
				num += num4;
				num2 += num4;
				list.AddRange(operateResult3.Content1);
			}
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	[HslMqttApi("ReadMultiAddress", "")]
	public OperateResult<byte[]> Read(string[] address, ushort[] length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[], ushort, bool> operateResult = ReadWithType(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content1);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		if (length == 1 && !Regex.IsMatch(address, "\\[[0-9]+\\]$"))
		{
			OperateResult<byte[]> operateResult = Read(address, length);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult);
			}
			return OperateResult.CreateSuccessResult(SoftBasic.ByteToBoolArray(operateResult.Content));
		}
		OperateResult<byte[]> operateResult2 = Read(address, length);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.Select((byte m) => m != 0).Take(length).ToArray());
	}

	[HslMqttApi("ReadByte", "")]
	public OperateResult<byte> ReadByte(string address)
	{
		return ByteTransformHelper.GetResultFromArray(Read(address, 1));
	}

	public OperateResult<ushort, byte[]> ReadTag(string address, ushort length = 1)
	{
		OperateResult<byte[], ushort, bool> operateResult = ReadWithType(new string[1] { address }, new ushort[1] { length });
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort, byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content2, operateResult.Content1);
	}

	[HslMqttApi(Description = "获取PLC的型号信息")]
	public OperateResult<string> ReadPlcType()
	{
		return OperateResult.CreateSuccessResult(ProductName);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		if (length == 1 && !Regex.IsMatch(address, "\\[[0-9]+\\]$"))
		{
			OperateResult<byte[]> read = await ReadAsync(address, length);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read);
			}
			return OperateResult.CreateSuccessResult(SoftBasic.ByteToBoolArray(read.Content));
		}
		OperateResult<byte[]> read2 = await ReadAsync(address, length);
		if (!read2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read2);
		}
		return OperateResult.CreateSuccessResult(read2.Content.Select((byte m) => m != 0).Take(length).ToArray());
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		HslHelper.ExtractParameter(ref address, "type", 0);
		if (length == 1)
		{
			OperateResult<byte[], ushort, bool> read3 = await ReadWithTypeAsync(new string[1] { address }, new ushort[1] { length });
			if (!read3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(read3);
			}
			return OperateResult.CreateSuccessResult(read3.Content1);
		}
		int count = 0;
		int index = 0;
		string format = "[{0}]";
		List<byte> array = new List<byte>();
		Match match = Regex.Match(address, "\\[[0-9]+\\]$");
		if (match.Success)
		{
			address = address.Remove(match.Index, match.Length);
			index = int.Parse(match.Value.Substring(1, match.Value.Length - 2));
		}
		else
		{
			Match match2 = Regex.Match(address, "\\[[0-9]+,[0-9]+\\]$");
			if (match2.Success)
			{
				address = address.Remove(match2.Index, match2.Length);
				string index2 = Regex.Matches(match2.Value, "[0-9]+")[1].Value;
				format = match2.Value.Replace(index2 + "]", "{0}]");
				index = int.Parse(index2);
			}
		}
		ushort dataType = 0;
		while (count < length)
		{
			if (count == 0)
			{
				ushort first = Math.Min(length, (ushort)248);
				OperateResult<byte[], ushort, bool> read4 = await ReadWithTypeAsync(new string[1] { address + string.Format(format, index) }, new ushort[1] { first });
				if (!read4.IsSuccess)
				{
					return OperateResult.CreateFailedResult<byte[]>(read4);
				}
				dataType = read4.Content2;
				count += first;
				index += first;
				array.AddRange(read4.Content1);
			}
			else
			{
				ushort len = (ushort)GetLengthFromRemain(dataType, length - count);
				OperateResult<byte[], ushort, bool> read5 = await ReadWithTypeAsync(new string[1] { address + string.Format(format, index) }, new ushort[1] { len });
				if (!read5.IsSuccess)
				{
					return OperateResult.CreateFailedResult<byte[]>(read5);
				}
				count += len;
				index += len;
				array.AddRange(read5.Content1);
			}
		}
		return OperateResult.CreateSuccessResult(array.ToArray());
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string[] address, ushort[] length)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[], ushort, bool> read = await ReadWithTypeAsync(address, length);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content1);
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadAsync(address, 1));
	}

	public async Task<OperateResult<ushort, byte[]>> ReadTagAsync(string address, ushort length = 1)
	{
		OperateResult<byte[], ushort, bool> read = await ReadWithTypeAsync(new string[1] { address }, new ushort[1] { length });
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort, byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content2, read.Content1);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return WriteTag(address, 209, value, (!HslHelper.IsAddressEndWithIndex(address)) ? 1 : value.Length);
	}

	public virtual OperateResult WriteTag(string address, ushort typeCode, byte[] value, int length = 1)
	{
		typeCode = (ushort)HslHelper.ExtractParameter(ref address, "type", typeCode);
		OperateResult<byte[]> operateResult = BuildWriteCommand(address, typeCode, value, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = AllenBradleyHelper.CheckResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult3);
		}
		return AllenBradleyHelper.ExtractActualData(operateResult2.Content, isRead: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await WriteTagAsync(address, 209, value, (!HslHelper.IsAddressEndWithIndex(address)) ? 1 : value.Length);
	}

	public virtual async Task<OperateResult> WriteTagAsync(string address, ushort typeCode, byte[] value, int length = 1)
	{
		typeCode = (ushort)HslHelper.ExtractParameter(ref address, "type", typeCode);
		OperateResult<byte[]> command = BuildWriteCommand(address, typeCode, value, length);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = AllenBradleyHelper.CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		return AllenBradleyHelper.ExtractActualData(read.Content, isRead: false);
	}

	[HslMqttApi("ReadInt16Array", "")]
	public override OperateResult<short[]> ReadInt16(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => base.ByteTransform.TransInt16(m, 0, length));
	}

	[HslMqttApi("ReadUInt16Array", "")]
	public override OperateResult<ushort[]> ReadUInt16(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => base.ByteTransform.TransUInt16(m, 0, length));
	}

	[HslMqttApi("ReadInt32Array", "")]
	public override OperateResult<int[]> ReadInt32(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => base.ByteTransform.TransInt32(m, 0, length));
	}

	[HslMqttApi("ReadUInt32Array", "")]
	public override OperateResult<uint[]> ReadUInt32(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => base.ByteTransform.TransUInt32(m, 0, length));
	}

	[HslMqttApi("ReadFloatArray", "")]
	public override OperateResult<float[]> ReadFloat(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => base.ByteTransform.TransSingle(m, 0, length));
	}

	[HslMqttApi("ReadInt64Array", "")]
	public override OperateResult<long[]> ReadInt64(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => base.ByteTransform.TransInt64(m, 0, length));
	}

	[HslMqttApi("ReadUInt64Array", "")]
	public override OperateResult<ulong[]> ReadUInt64(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => base.ByteTransform.TransUInt64(m, 0, length));
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => base.ByteTransform.TransDouble(m, 0, length));
	}

	public OperateResult<string> ReadString(string address)
	{
		return ReadString(address, 1, Encoding.UTF8);
	}

	[HslMqttApi("ReadString", "")]
	public override OperateResult<string> ReadString(string address, ushort length)
	{
		return ReadString(address, length, Encoding.UTF8);
	}

	public override OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		OperateResult<byte[]> operateResult = Read(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return ExtraStringContent(operateResult.Content, encoding);
	}

	private OperateResult<string> ExtraStringContent(byte[] content, Encoding encoding)
	{
		try
		{
			if (content.Length >= 2)
			{
				int count = base.ByteTransform.TransUInt16(content, 0);
				return OperateResult.CreateSuccessResult(encoding.GetString(content, 2, count));
			}
			return OperateResult.CreateSuccessResult(encoding.GetString(content));
		}
		catch (Exception ex)
		{
			return new OperateResult<string>("Parse string failed: " + ex.Message + " Source: " + content.ToHexString(' '));
		}
	}

	public override async Task<OperateResult<short[]>> ReadInt16Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => base.ByteTransform.TransInt16(m, 0, length));
	}

	public override async Task<OperateResult<ushort[]>> ReadUInt16Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => base.ByteTransform.TransUInt16(m, 0, length));
	}

	public override async Task<OperateResult<int[]>> ReadInt32Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => base.ByteTransform.TransInt32(m, 0, length));
	}

	public override async Task<OperateResult<uint[]>> ReadUInt32Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => base.ByteTransform.TransUInt32(m, 0, length));
	}

	public override async Task<OperateResult<float[]>> ReadFloatAsync(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => base.ByteTransform.TransSingle(m, 0, length));
	}

	public override async Task<OperateResult<long[]>> ReadInt64Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => base.ByteTransform.TransInt64(m, 0, length));
	}

	public override async Task<OperateResult<ulong[]>> ReadUInt64Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => base.ByteTransform.TransUInt64(m, 0, length));
	}

	public override async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => base.ByteTransform.TransDouble(m, 0, length));
	}

	public async Task<OperateResult<string>> ReadStringAsync(string address)
	{
		return await ReadStringAsync(address, 1, Encoding.UTF8);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length)
	{
		return await ReadStringAsync(address, length, Encoding.UTF8);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		OperateResult<byte[]> read = await ReadAsync(address, length);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		return ExtraStringContent(read.Content, encoding);
	}

	[HslMqttApi("WriteInt16Array", "")]
	public override OperateResult Write(string address, short[] values)
	{
		return WriteTag(address, 195, base.ByteTransform.TransByte(values), values.Length);
	}

	[HslMqttApi("WriteUInt16Array", "")]
	public override OperateResult Write(string address, ushort[] values)
	{
		return WriteTag(address, 199, base.ByteTransform.TransByte(values), values.Length);
	}

	[HslMqttApi("WriteInt32Array", "")]
	public override OperateResult Write(string address, int[] values)
	{
		return WriteTag(address, 196, base.ByteTransform.TransByte(values), values.Length);
	}

	[HslMqttApi("WriteUInt32Array", "")]
	public override OperateResult Write(string address, uint[] values)
	{
		return WriteTag(address, 200, base.ByteTransform.TransByte(values), values.Length);
	}

	[HslMqttApi("WriteFloatArray", "")]
	public override OperateResult Write(string address, float[] values)
	{
		return WriteTag(address, 202, base.ByteTransform.TransByte(values), values.Length);
	}

	[HslMqttApi("WriteInt64Array", "")]
	public override OperateResult Write(string address, long[] values)
	{
		return WriteTag(address, 197, base.ByteTransform.TransByte(values), values.Length);
	}

	[HslMqttApi("WriteUInt64Array", "")]
	public override OperateResult Write(string address, ulong[] values)
	{
		return WriteTag(address, 201, base.ByteTransform.TransByte(values), values.Length);
	}

	[HslMqttApi("WriteDoubleArray", "")]
	public override OperateResult Write(string address, double[] values)
	{
		return WriteTag(address, 203, base.ByteTransform.TransByte(values), values.Length);
	}

	[HslMqttApi("WriteString", "")]
	public override OperateResult Write(string address, string value)
	{
		return Write(address, value, Encoding.UTF8);
	}

	public override OperateResult Write(string address, string value, Encoding encoding)
	{
		byte[] array = (string.IsNullOrEmpty(value) ? new byte[0] : encoding.GetBytes(value));
		return WriteTag(address, 208, SoftBasic.SpliceArray<byte>(BitConverter.GetBytes((ushort)array.Length), array));
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		return WriteTag(address, 193, (!value) ? new byte[2] : new byte[2] { 255, 255 });
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return WriteTag(address, 193, value.Select((bool m) => (byte)(m ? 1u : 0u)).ToArray(), (!HslHelper.IsAddressEndWithIndex(address)) ? 1 : value.Length);
	}

	[HslMqttApi("WriteByte", "")]
	public OperateResult Write(string address, byte value)
	{
		return WriteTag(address, 194, new byte[1] { value });
	}

	public override async Task<OperateResult> WriteAsync(string address, short[] values)
	{
		return await WriteTagAsync(address, 195, base.ByteTransform.TransByte(values), values.Length);
	}

	public override async Task<OperateResult> WriteAsync(string address, ushort[] values)
	{
		return await WriteTagAsync(address, 199, base.ByteTransform.TransByte(values), values.Length);
	}

	public override async Task<OperateResult> WriteAsync(string address, int[] values)
	{
		return await WriteTagAsync(address, 196, base.ByteTransform.TransByte(values), values.Length);
	}

	public override async Task<OperateResult> WriteAsync(string address, uint[] values)
	{
		return await WriteTagAsync(address, 200, base.ByteTransform.TransByte(values), values.Length);
	}

	public override async Task<OperateResult> WriteAsync(string address, float[] values)
	{
		return await WriteTagAsync(address, 202, base.ByteTransform.TransByte(values), values.Length);
	}

	public override async Task<OperateResult> WriteAsync(string address, long[] values)
	{
		return await WriteTagAsync(address, 197, base.ByteTransform.TransByte(values), values.Length);
	}

	public override async Task<OperateResult> WriteAsync(string address, ulong[] values)
	{
		return await WriteTagAsync(address, 201, base.ByteTransform.TransByte(values), values.Length);
	}

	public override async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await WriteTagAsync(address, 203, base.ByteTransform.TransByte(values), values.Length);
	}

	public override async Task<OperateResult> WriteAsync(string address, string value)
	{
		return await WriteAsync(address, value, Encoding.UTF8);
	}

	public override async Task<OperateResult> WriteAsync(string address, string value, Encoding encoding)
	{
		byte[] buffer = (string.IsNullOrEmpty(value) ? new byte[0] : encoding.GetBytes(value));
		return await WriteTagAsync(address, 208, SoftBasic.SpliceArray<byte>(BitConverter.GetBytes((ushort)buffer.Length), buffer));
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await WriteTagAsync(address, 193, (!value) ? new byte[2] : new byte[2] { 255, 255 });
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await WriteTagAsync(address, 193, value.Select((bool m) => (byte)(m ? 1u : 0u)).ToArray(), (!HslHelper.IsAddressEndWithIndex(address)) ? 1 : value.Length);
	}

	public async Task<OperateResult> WriteAsync(string address, byte value)
	{
		return await WriteTagAsync(address, 194, new byte[1] { value });
	}

	public OperateResult<DateTime> ReadDate(string address)
	{
		return AllenBradleyHelper.ReadDate(this, address);
	}

	public OperateResult WriteDate(string address, DateTime date)
	{
		return AllenBradleyHelper.WriteDate(this, address, date);
	}

	public OperateResult WriteTimeAndDate(string address, DateTime date)
	{
		return AllenBradleyHelper.WriteTimeAndDate(this, address, date);
	}

	public OperateResult<TimeSpan> ReadTime(string address)
	{
		return AllenBradleyHelper.ReadTime(this, address);
	}

	public OperateResult WriteTime(string address, TimeSpan time)
	{
		return AllenBradleyHelper.WriteTime(this, address, time);
	}

	public OperateResult WriteTimeOfDate(string address, TimeSpan timeOfDate)
	{
		return AllenBradleyHelper.WriteTimeOfDate(this, address, timeOfDate);
	}

	public async Task<OperateResult<DateTime>> ReadDateAsync(string address)
	{
		return await AllenBradleyHelper.ReadDateAsync(this, address);
	}

	public async Task<OperateResult> WriteDateAsync(string address, DateTime date)
	{
		return await AllenBradleyHelper.WriteDateAsync(this, address, date);
	}

	public async Task<OperateResult> WriteTimeAndDateAsync(string address, DateTime date)
	{
		return await AllenBradleyHelper.WriteTimeAndDateAsync(this, address, date);
	}

	public async Task<OperateResult<TimeSpan>> ReadTimeAsync(string address)
	{
		return await AllenBradleyHelper.ReadTimeAsync(this, address);
	}

	public async Task<OperateResult> WriteTimeAsync(string address, TimeSpan time)
	{
		return await AllenBradleyHelper.WriteTimeAsync(this, address, time);
	}

	public async Task<OperateResult> WriteTimeOfDateAsync(string address, TimeSpan timeOfDate)
	{
		return await AllenBradleyHelper.WriteTimeOfDateAsync(this, address, timeOfDate);
	}

	public override string ToString()
	{
		return $"OmronConnectedCipNet[{IpAddress}:{Port}]";
	}
}
