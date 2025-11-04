using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.AllenBradley;

public class AllenBradleyNet : DeviceTcpNet, IReadWriteCip, IReadWriteNet
{
	private long contextId = 0L;

	public uint SessionHandle { get; protected set; }

	public byte Slot { get; set; } = 0;

	public byte[] PortSlot { get; set; }

	public ushort CipCommand { get; set; } = 111;

	public MessageRouter MessageRouter { get; set; }

	public bool ContextCheck { get; set; } = false;

	public bool ReadArrayUseSegment { get; set; } = true;

	public AllenBradleyNet()
	{
		base.WordLength = 2;
		base.ByteTransform = new RegularByteTransform();
	}

	public AllenBradleyNet(string ipAddress, int port = 44818)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new AllenBradleyMessage(ContextCheck);
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		byte[] bytes = BitConverter.GetBytes(Interlocked.Increment(ref contextId));
		return AllenBradleyHelper.PackRequestHeader(CipCommand, SessionHandle, command, bytes);
	}

	protected override OperateResult InitializationOnConnect()
	{
		Interlocked.Exchange(ref contextId, 0L);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, AllenBradleyHelper.RegisterSessionHandle(), hasResponseData: true, usePackAndUnpack: false);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = AllenBradleyHelper.CheckResponse(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		if (operateResult.Content.Length >= 8)
		{
			SessionHandle = BitConverter.ToUInt32(operateResult.Content, 4);
		}
		if (MessageRouter != null)
		{
			byte[] routerCIP = MessageRouter.GetRouterCIP();
			OperateResult<byte[]> operateResult3 = ReadFromCoreServer(CommunicationPipe, AllenBradleyHelper.PackRequestHeader(111, SessionHandle, AllenBradleyHelper.PackCommandSpecificData(new byte[4], AllenBradleyHelper.PackCommandSingleService(routerCIP, 178, isConnected: false, 0))), hasResponseData: true, usePackAndUnpack: false);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override OperateResult ExtraOnDisconnect()
	{
		if (CommunicationPipe != null)
		{
			OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, AllenBradleyHelper.UnRegisterSessionHandle(SessionHandle), hasResponseData: true, usePackAndUnpack: false);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		Interlocked.Exchange(ref contextId, 0L);
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(CommunicationPipe, AllenBradleyHelper.RegisterSessionHandle(), hasResponseData: true, usePackAndUnpack: false).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = AllenBradleyHelper.CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return check;
		}
		if (read.Content.Length >= 8)
		{
			SessionHandle = BitConverter.ToUInt32(read.Content, 4);
		}
		if (MessageRouter != null)
		{
			byte[] cip = MessageRouter.GetRouterCIP();
			OperateResult<byte[]> messageRouter = await ReadFromCoreServerAsync(CommunicationPipe, AllenBradleyHelper.PackRequestHeader(111, SessionHandle, AllenBradleyHelper.PackCommandSpecificData(new byte[4], AllenBradleyHelper.PackCommandSingleService(cip, 178, isConnected: false, 0))), hasResponseData: true, usePackAndUnpack: false).ConfigureAwait(continueOnCapturedContext: false);
			if (!messageRouter.IsSuccess)
			{
				return messageRouter;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> ExtraOnDisconnectAsync()
	{
		if (CommunicationPipe != null)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(CommunicationPipe, AllenBradleyHelper.UnRegisterSessionHandle(SessionHandle), hasResponseData: true, usePackAndUnpack: false);
			if (!read.IsSuccess)
			{
				return read;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public virtual OperateResult<byte[]> BuildReadCommand(string[] address, ushort[] length)
	{
		if (address == null || length == null)
		{
			return new OperateResult<byte[]>("address or length is null");
		}
		if (address.Length != length.Length)
		{
			return new OperateResult<byte[]>("address and length is not same array");
		}
		try
		{
			byte b = Slot;
			List<byte[]> list = new List<byte[]>();
			for (int i = 0; i < address.Length; i++)
			{
				b = (byte)HslHelper.ExtractParameter(ref address[i], "slot", Slot);
				list.Add(AllenBradleyHelper.PackRequsetRead(address[i], length[i]));
			}
			byte[] value = AllenBradleyHelper.PackCommandSpecificData(new byte[4], PackCommandService(PortSlot ?? new byte[2] { 1, b }, list.ToArray()));
			return OperateResult.CreateSuccessResult(value);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Address Wrong:" + ex.Message);
		}
	}

	public OperateResult<byte[]> BuildReadCommand(string[] address)
	{
		if (address == null)
		{
			return new OperateResult<byte[]>("address or length is null");
		}
		ushort[] array = new ushort[address.Length];
		for (int i = 0; i < address.Length; i++)
		{
			array[i] = 1;
		}
		return BuildReadCommand(address, array);
	}

	protected virtual OperateResult<List<byte[]>> BuildWriteCommand(string address, ushort typeCode, byte[] data, int length = 1)
	{
		try
		{
			byte b = (byte)HslHelper.ExtractParameter(ref address, "slot", Slot);
			int num = HslHelper.ExtractParameter(ref address, "x", -1);
			if (num == 83 || num == 82)
			{
				int num2 = 0;
				List<byte[]> list = SoftBasic.ArraySplitByLength(data, 474);
				for (int i = 0; i < list.Count; i++)
				{
					byte[] array = AllenBradleyHelper.PackRequestWriteSegment(address, typeCode, list[i], num2, length);
					num2 += list[i].Length;
					byte[] array2 = (list[i] = AllenBradleyHelper.PackCommandSpecificData(new byte[4], PackCommandService(PortSlot ?? new byte[2] { 1, b }, array)));
					byte[] array4 = array2;
				}
				return OperateResult.CreateSuccessResult(list);
			}
			byte[] array5 = AllenBradleyHelper.PackRequestWrite(address, typeCode, data, length, isConnectedAddress: false, GetBoolWritePadding());
			byte[] item = AllenBradleyHelper.PackCommandSpecificData(new byte[4], PackCommandService(PortSlot ?? new byte[2] { 1, b }, array5));
			return OperateResult.CreateSuccessResult(new List<byte[]> { item });
		}
		catch (Exception ex)
		{
			return new OperateResult<List<byte[]>>("Address Wrong:" + ex.Message);
		}
	}

	protected virtual bool GetBoolWritePadding()
	{
		return false;
	}

	public OperateResult<byte[]> BuildWriteCommand(string address, bool data)
	{
		try
		{
			byte b = (byte)HslHelper.ExtractParameter(ref address, "slot", Slot);
			byte[] array = AllenBradleyHelper.PackRequestWrite(address, data);
			byte[] value = AllenBradleyHelper.PackCommandSpecificData(new byte[4], PackCommandService(PortSlot ?? new byte[2] { 1, b }, array));
			return OperateResult.CreateSuccessResult(value);
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Address Wrong:" + ex.Message);
		}
	}

	private OperateResult CheckResponse(byte[] response)
	{
		OperateResult operateResult = AllenBradleyHelper.CheckResponse(response);
		if (!operateResult.IsSuccess && operateResult.ErrorCode == 100)
		{
			CommunicationPipe.RaisePipeError();
		}
		return operateResult;
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		HslHelper.ExtractParameter(ref address, "type", 0);
		int num = HslHelper.ExtractParameter(ref address, "x", -1);
		if (num == 82 || num == 83)
		{
			return ReadSegment(address, 0, length);
		}
		if (length > 1 && ReadArrayUseSegment)
		{
			return ReadSegment(address, 0, length);
		}
		return Read(new string[1] { address }, new ushort[1] { length });
	}

	[HslMqttApi("ReadAddress", "")]
	public OperateResult<byte[]> Read(string[] address)
	{
		if (address == null)
		{
			return new OperateResult<byte[]>("address can not be null");
		}
		ushort[] array = new ushort[address.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 1;
		}
		return Read(address, array);
	}

	public OperateResult<byte[]> Read(string[] address, ushort[] length)
	{
		if (address != null && address.Length > 1 && !Authorization.asdniasnfaksndiqwhawfskhfaiw())
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
		OperateResult operateResult3 = CheckResponse(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(operateResult3);
		}
		return AllenBradleyHelper.ExtractActualData(operateResult2.Content, isRead: true);
	}

	[HslMqttApi("ReadSegment", "")]
	public OperateResult<byte[]> ReadSegment(string address, int startIndex, int length)
	{
		try
		{
			List<byte> list = new List<byte>();
			OperateResult<byte[], ushort, bool> operateResult2;
			do
			{
				OperateResult<byte[]> operateResult = ReadCipFromServer(AllenBradleyHelper.PackRequestReadSegment(address, startIndex, length));
				if (!operateResult.IsSuccess)
				{
					return operateResult;
				}
				operateResult2 = AllenBradleyHelper.ExtractActualData(operateResult.Content, isRead: true);
				if (!operateResult2.IsSuccess)
				{
					return OperateResult.CreateFailedResult<byte[]>(operateResult2);
				}
				startIndex += operateResult2.Content1.Length;
				list.AddRange(operateResult2.Content1);
			}
			while (operateResult2.Content3);
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<byte[]>("Address Wrong:" + ex.Message);
		}
	}

	private OperateResult<byte[]> ReadByCips(params byte[][] cips)
	{
		OperateResult<byte[]> operateResult = ReadCipFromServer(cips);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[], ushort, bool> operateResult2 = AllenBradleyHelper.ExtractActualData(operateResult.Content, isRead: true);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content1);
	}

	public OperateResult<byte[]> ReadCipFromServer(params byte[][] cips)
	{
		byte[] send = AllenBradleyHelper.PackCommandSpecificData(new byte[4], PackCommandService(PortSlot ?? new byte[2] { 1, Slot }, cips.ToArray()));
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = CheckResponse(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content);
	}

	public OperateResult<byte[]> ReadEipFromServer(params byte[][] eip)
	{
		byte[] send = AllenBradleyHelper.PackCommandSpecificData(eip);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = CheckResponse(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content);
	}

	[HslMqttApi("ReadBool", "")]
	public override OperateResult<bool> ReadBool(string address)
	{
		if (address.StartsWith("i="))
		{
			return ByteTransformHelper.GetResultFromArray(ReadBool(address, 1));
		}
		OperateResult<byte[]> operateResult = Read(address, 1);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult);
		}
		return OperateResult.CreateSuccessResult(base.ByteTransform.TransBool(operateResult.Content, 0));
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		if (address.StartsWith("i="))
		{
			address = address.Substring(2);
			address = AllenBradleyHelper.AnalysisArrayIndex(address, out var arrayIndex);
			string text = ((arrayIndex / 32 == 0) ? "" : $"[{arrayIndex / 32}]");
			ushort length2 = (ushort)HslHelper.CalculateOccupyLength(arrayIndex, length, 32);
			OperateResult<byte[]> operateResult = Read(address + text, length2);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(operateResult);
			}
			return OperateResult.CreateSuccessResult(operateResult.Content.ToBoolArray().SelectMiddle(arrayIndex % 32, length));
		}
		OperateResult<byte[]> operateResult2 = Read(address, length);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.ByteToBoolArray(operateResult2.Content, length));
	}

	[HslMqttApi("ReadBoolArrayAddress", "")]
	public OperateResult<bool[]> ReadBoolArray(string address)
	{
		OperateResult<byte[]> operateResult = Read(address, 1);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.ToBoolArray());
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

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		HslHelper.ExtractParameter(ref address, "type", 0);
		int x = HslHelper.ExtractParameter(ref address, "x", -1);
		if (x == 82 || x == 83)
		{
			return await ReadSegmentAsync(address, 0, length).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (length > 1 && ReadArrayUseSegment)
		{
			return await ReadSegmentAsync(address, 0, length).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await ReadAsync(new string[1] { address }, new ushort[1] { length }).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string[] address)
	{
		if (address == null)
		{
			return new OperateResult<byte[]>("address can not be null");
		}
		ushort[] length = new ushort[address.Length];
		for (int i = 0; i < length.Length; i++)
		{
			length[i] = 1;
		}
		return await ReadAsync(address, length);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string[] address, ushort[] length)
	{
		if (address != null && address.Length > 1 && !Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<byte[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<byte[], ushort, bool> read = await ReadWithTypeAsync(address, length).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content1);
	}

	private async Task<OperateResult<byte[], ushort, bool>> ReadWithTypeAsync(string[] address, ushort[] length)
	{
		OperateResult<byte[]> command = BuildReadCommand(address, length);
		if (!command.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(command);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(read);
		}
		OperateResult check = CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[], ushort, bool>(check);
		}
		return AllenBradleyHelper.ExtractActualData(read.Content, isRead: true);
	}

	public async Task<OperateResult<byte[]>> ReadSegmentAsync(string address, int startIndex, int length)
	{
		try
		{
			List<byte> bytesContent = new List<byte>();
			OperateResult<byte[], ushort, bool> analysis;
			do
			{
				OperateResult<byte[]> read = await ReadCipFromServerAsync(AllenBradleyHelper.PackRequestReadSegment(address, startIndex, length)).ConfigureAwait(continueOnCapturedContext: false);
				if (!read.IsSuccess)
				{
					return read;
				}
				analysis = AllenBradleyHelper.ExtractActualData(read.Content, isRead: true);
				if (!analysis.IsSuccess)
				{
					return OperateResult.CreateFailedResult<byte[]>(analysis);
				}
				startIndex += analysis.Content1.Length;
				bytesContent.AddRange(analysis.Content1);
			}
			while (analysis.Content3);
			return OperateResult.CreateSuccessResult(bytesContent.ToArray());
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			return new OperateResult<byte[]>("Address Wrong:" + ex3.Message);
		}
	}

	public async Task<OperateResult<byte[]>> ReadCipFromServerAsync(params byte[][] cips)
	{
		byte[] commandSpecificData = AllenBradleyHelper.PackCommandSpecificData(new byte[4], PackCommandService(PortSlot ?? new byte[2] { 1, Slot }, cips.ToArray()));
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(commandSpecificData).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		return OperateResult.CreateSuccessResult(read.Content);
	}

	public async Task<OperateResult<byte[]>> ReadEipFromServerAsync(params byte[][] eip)
	{
		byte[] commandSpecificData = AllenBradleyHelper.PackCommandSpecificData(eip);
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(commandSpecificData).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		OperateResult check = CheckResponse(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(check);
		}
		return OperateResult.CreateSuccessResult(read.Content);
	}

	public override async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		if (address.StartsWith("i="))
		{
			return ByteTransformHelper.GetResultFromArray(await ReadBoolAsync(address, 1).ConfigureAwait(continueOnCapturedContext: false));
		}
		OperateResult<byte[]> read = await ReadAsync(address, 1).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(read);
		}
		return OperateResult.CreateSuccessResult(base.ByteTransform.TransBool(read.Content, 0));
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		if (address.StartsWith("i="))
		{
			address = address.Substring(2);
			address = AllenBradleyHelper.AnalysisArrayIndex(address, out var bitIndex);
			string uintIndex = ((bitIndex / 32 == 0) ? "" : $"[{bitIndex / 32}]");
			OperateResult<byte[]> read2 = await ReadAsync(length: (ushort)HslHelper.CalculateOccupyLength(bitIndex, length, 32), address: address + uintIndex).ConfigureAwait(continueOnCapturedContext: false);
			if (!read2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<bool[]>(read2);
			}
			return OperateResult.CreateSuccessResult(read2.Content.ToBoolArray().SelectMiddle(bitIndex % 32, length));
		}
		OperateResult<byte[]> read3 = await ReadAsync(address, length).ConfigureAwait(continueOnCapturedContext: false);
		if (!read3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read3);
		}
		return OperateResult.CreateSuccessResult(SoftBasic.ByteToBoolArray(read3.Content, length));
	}

	public async Task<OperateResult<bool[]>> ReadBoolArrayAsync(string address)
	{
		OperateResult<byte[]> read = await ReadAsync(address, 1).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.ToBoolArray());
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadAsync(address, 1));
	}

	public async Task<OperateResult<ushort, byte[]>> ReadTagAsync(string address, ushort length = 1)
	{
		OperateResult<byte[], ushort, bool> read = await ReadWithTypeAsync(new string[1] { address }, new ushort[1] { length }).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort, byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content2, read.Content1);
	}

	public OperateResult<AbTagItem[]> TagEnumerator()
	{
		List<AbTagItem> list = new List<AbTagItem>();
		for (int i = 0; i < 2; i++)
		{
			uint startInstance = 0u;
			OperateResult<byte[], ushort, bool> operateResult2;
			do
			{
				OperateResult<byte[]> operateResult = ReadCipFromServer((i == 0) ? AllenBradleyHelper.BuildEnumeratorCommand(startInstance) : AllenBradleyHelper.BuildEnumeratorProgrameMainCommand(startInstance));
				if (!operateResult.IsSuccess)
				{
					return OperateResult.CreateFailedResult<AbTagItem[]>(operateResult);
				}
				operateResult2 = AllenBradleyHelper.ExtractActualData(operateResult.Content, isRead: true);
				if (!operateResult2.IsSuccess)
				{
					if (i == 1)
					{
						return OperateResult.CreateSuccessResult(list.ToArray());
					}
					return OperateResult.CreateFailedResult<AbTagItem[]>(operateResult2);
				}
				if (operateResult.Content.Length >= 43 && BitConverter.ToUInt16(operateResult.Content, 40) == 213)
				{
					list.AddRange(AbTagItem.PraseAbTagItems(operateResult.Content, 44, i == 0, out var instance));
					startInstance = instance + 1;
					continue;
				}
				return new OperateResult<AbTagItem[]>(StringResources.Language.UnknownError + " Source: " + operateResult.Content.ToHexString(' '));
			}
			while (operateResult2.Content3);
		}
		return OperateResult.CreateSuccessResult(list.ToArray());
	}

	public async Task<OperateResult<AbTagItem[]>> TagEnumeratorAsync()
	{
		List<AbTagItem> lists = new List<AbTagItem>();
		for (int i = 0; i < 2; i++)
		{
			uint instanceAddress = 0u;
			OperateResult<byte[], ushort, bool> analysis;
			do
			{
				OperateResult<byte[]> readCip = await ReadCipFromServerAsync((i == 0) ? AllenBradleyHelper.BuildEnumeratorCommand(instanceAddress) : AllenBradleyHelper.BuildEnumeratorProgrameMainCommand(instanceAddress));
				if (!readCip.IsSuccess)
				{
					return OperateResult.CreateFailedResult<AbTagItem[]>(readCip);
				}
				analysis = AllenBradleyHelper.ExtractActualData(readCip.Content, isRead: true);
				if (!analysis.IsSuccess)
				{
					if (i == 1)
					{
						return OperateResult.CreateSuccessResult(lists.ToArray());
					}
					return OperateResult.CreateFailedResult<AbTagItem[]>(analysis);
				}
				if (readCip.Content.Length >= 43 && BitConverter.ToUInt16(readCip.Content, 40) == 213)
				{
					lists.AddRange(AbTagItem.PraseAbTagItems(readCip.Content, 44, i == 0, out var instance));
					instanceAddress = instance + 1;
					continue;
				}
				return new OperateResult<AbTagItem[]>(StringResources.Language.UnknownError + " Source: " + readCip.Content.ToHexString(' '));
			}
			while (analysis.Content3);
		}
		return OperateResult.CreateSuccessResult(lists.ToArray());
	}

	private OperateResult<AbStructHandle> ReadTagStructHandle(AbTagItem structTag)
	{
		OperateResult<byte[]> operateResult = ReadCipFromServer(AllenBradleyHelper.GetStructHandleCommand(structTag.SymbolType));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<AbStructHandle>(operateResult);
		}
		if (operateResult.Content.Length >= 43 && BitConverter.ToInt32(operateResult.Content, 40) == 131)
		{
			return OperateResult.CreateSuccessResult(new AbStructHandle(operateResult.Content, 44));
		}
		return new OperateResult<AbStructHandle>(StringResources.Language.UnknownError + " Source Data: " + operateResult.Content.ToHexString(' '));
	}

	public OperateResult<AbTagItem[]> StructTagEnumerator(AbTagItem structTag)
	{
		OperateResult<AbStructHandle> operateResult = ReadTagStructHandle(structTag);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<AbTagItem[]>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadCipFromServer(AllenBradleyHelper.GetStructItemNameType(structTag.SymbolType, operateResult.Content, 0));
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<AbTagItem[]>(operateResult2);
		}
		if (operateResult2.Content.Length >= 43 && operateResult2.Content[40] == 204 && operateResult2.Content[41] == 0 && operateResult2.Content[42] == 0)
		{
			return OperateResult.CreateSuccessResult(AbTagItem.PraseAbTagItemsFromStruct(operateResult2.Content, 44, operateResult.Content).ToArray());
		}
		return new OperateResult<AbTagItem[]>(StringResources.Language.UnknownError + " Status:" + operateResult2.Content[42]);
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
		return ReadString(address, 1);
	}

	[HslMqttApi("ReadString", "")]
	public override OperateResult<string> ReadString(string address, ushort length)
	{
		return ReadString(address, length, Encoding.UTF8);
	}

	public override OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		HslHelper.ExtractParameter(ref address, "type", 0);
		OperateResult<byte[], ushort, bool> read = ReadWithType(new string[1] { address }, new ushort[1] { length });
		return AllenBradleyHelper.ExtractActualString(read, base.ByteTransform, encoding);
	}

	[HslMqttApi(Description = "获取PLC的型号信息")]
	public OperateResult<string> ReadPlcType()
	{
		return AllenBradleyHelper.ReadPlcType(this);
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
		return await ReadStringAsync(address, 1);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length)
	{
		return await ReadStringAsync(address, length, Encoding.UTF8);
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		HslHelper.ExtractParameter(ref address, "type", 0);
		return AllenBradleyHelper.ExtractActualString(await ReadWithTypeAsync(new string[1] { address }, new ushort[1] { length }), base.ByteTransform, encoding);
	}

	public async Task<OperateResult<string>> ReadPlcTypeAsync()
	{
		return await AllenBradleyHelper.ReadPlcTypeAsync(this);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return WriteTag(address, 209, value, (!HslHelper.IsAddressEndWithIndex(address)) ? 1 : value.Length);
	}

	public virtual OperateResult WriteTag(string address, ushort typeCode, byte[] value, int length = 1)
	{
		typeCode = (ushort)HslHelper.ExtractParameter(ref address, "type", typeCode);
		OperateResult<List<byte[]>> operateResult = BuildWriteCommand(address, typeCode, value, length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		for (int i = 0; i < operateResult.Content.Count; i++)
		{
			OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content[i]);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult operateResult3 = CheckResponse(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult3);
			}
			OperateResult operateResult4 = AllenBradleyHelper.ExtractActualData(operateResult2.Content, isRead: false);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await WriteTagAsync(address, 209, value, (!HslHelper.IsAddressEndWithIndex(address)) ? 1 : value.Length);
	}

	public virtual async Task<OperateResult> WriteTagAsync(string address, ushort typeCode, byte[] value, int length = 1)
	{
		typeCode = (ushort)HslHelper.ExtractParameter(ref address, "type", typeCode);
		OperateResult<List<byte[]>> command = BuildWriteCommand(address, typeCode, value, length);
		if (!command.IsSuccess)
		{
			return command;
		}
		for (int i = 0; i < command.Content.Count; i++)
		{
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content[i]).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				return read;
			}
			OperateResult check = CheckResponse(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(check);
			}
			OperateResult extra = AllenBradleyHelper.ExtractActualData(read.Content, isRead: false);
			if (!extra.IsSuccess)
			{
				return extra;
			}
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi("WriteInt16Array", "")]
	public override OperateResult Write(string address, short[] values)
	{
		return WriteTag(address, 195, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length));
	}

	[HslMqttApi("WriteUInt16Array", "")]
	public override OperateResult Write(string address, ushort[] values)
	{
		return WriteTag(address, 199, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length));
	}

	[HslMqttApi("WriteInt32Array", "")]
	public override OperateResult Write(string address, int[] values)
	{
		return WriteTag(address, 196, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length));
	}

	[HslMqttApi("WriteUInt32Array", "")]
	public override OperateResult Write(string address, uint[] values)
	{
		return WriteTag(address, 200, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length));
	}

	[HslMqttApi("WriteFloatArray", "")]
	public override OperateResult Write(string address, float[] values)
	{
		return WriteTag(address, 202, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length));
	}

	[HslMqttApi("WriteInt64Array", "")]
	public override OperateResult Write(string address, long[] values)
	{
		return WriteTag(address, 197, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length));
	}

	[HslMqttApi("WriteUInt64Array", "")]
	public override OperateResult Write(string address, ulong[] values)
	{
		return WriteTag(address, 201, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length));
	}

	[HslMqttApi("WriteDoubleArray", "")]
	public override OperateResult Write(string address, double[] values)
	{
		return WriteTag(address, 203, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length));
	}

	public override OperateResult Write(string address, string value, Encoding encoding)
	{
		if (string.IsNullOrEmpty(value))
		{
			value = string.Empty;
		}
		ushort num = (ushort)HslHelper.ExtractParameter(ref address, "type", 194);
		if (num == 218)
		{
			byte[] bytes = encoding.GetBytes(value);
			return WriteTag(address, num, SoftBasic.SpliceArray<byte>(new byte[1] { (byte)bytes.Length }, bytes));
		}
		byte[] bytes2 = encoding.GetBytes(value);
		OperateResult operateResult = Write(address + ".LEN", bytes2.Length);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		byte[] value2 = SoftBasic.ArrayExpandToLengthEven(bytes2);
		return WriteTag(address + ".DATA[0]", num, value2, bytes2.Length);
	}

	[HslMqttApi("WriteBool", "")]
	public override OperateResult Write(string address, bool value)
	{
		if (address.StartsWith("i=") && Regex.IsMatch(address, "\\[[0-9]+\\]$"))
		{
			OperateResult<byte[]> operateResult = BuildWriteCommand(address.Substring(2), value);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult operateResult3 = CheckResponse(operateResult2.Content);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult3);
			}
			return AllenBradleyHelper.ExtractActualData(operateResult2.Content, isRead: false);
		}
		return WriteTag(address, 193, (!value) ? new byte[2] : new byte[2] { 255, 255 });
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		return WriteTag(address, 193, value.Select((bool m) => (byte)(m ? 1u : 0u)).ToArray(), (!HslHelper.IsAddressEndWithIndex(address)) ? 1 : value.Length);
	}

	[HslMqttApi("WriteByte", "")]
	public virtual OperateResult Write(string address, byte value)
	{
		return WriteTag(address, 194, new byte[2] { value, 0 });
	}

	public override async Task<OperateResult> WriteAsync(string address, short[] values)
	{
		return await WriteTagAsync(address, 195, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, ushort[] values)
	{
		return await WriteTagAsync(address, 199, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, int[] values)
	{
		return await WriteTagAsync(address, 196, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, uint[] values)
	{
		return await WriteTagAsync(address, 200, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, float[] values)
	{
		return await WriteTagAsync(address, 202, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, long[] values)
	{
		return await WriteTagAsync(address, 197, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, ulong[] values)
	{
		return await WriteTagAsync(address, 201, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await WriteTagAsync(address, 203, base.ByteTransform.TransByte(values), GetWriteValueLength(address, values.Length)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, string value, Encoding encoding)
	{
		if (string.IsNullOrEmpty(value))
		{
			value = string.Empty;
		}
		ushort typeCode = (ushort)HslHelper.ExtractParameter(ref address, "type", 194);
		if (typeCode == 218)
		{
			byte[] data2 = encoding.GetBytes(value);
			return WriteTag(address, typeCode, SoftBasic.SpliceArray<byte>(new byte[1] { (byte)data2.Length }, data2));
		}
		byte[] data3 = encoding.GetBytes(value);
		OperateResult write = await WriteAsync(address + ".LEN", data3.Length).ConfigureAwait(continueOnCapturedContext: false);
		if (!write.IsSuccess)
		{
			return write;
		}
		return await WriteTagAsync(value: SoftBasic.ArrayExpandToLengthEven(data3), address: address + ".DATA[0]", typeCode: typeCode, length: data3.Length).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool value)
	{
		if (address.StartsWith("i=") && Regex.IsMatch(address, "\\[[0-9]+\\]$"))
		{
			OperateResult<byte[]> command = BuildWriteCommand(address.Substring(2), value);
			if (!command.IsSuccess)
			{
				return command;
			}
			OperateResult<byte[]> read = await ReadFromCoreServerAsync(command.Content).ConfigureAwait(continueOnCapturedContext: false);
			if (!read.IsSuccess)
			{
				return read;
			}
			OperateResult check = CheckResponse(read.Content);
			if (!check.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(check);
			}
			return AllenBradleyHelper.ExtractActualData(read.Content, isRead: false);
		}
		return await WriteTagAsync(address, 193, (!value) ? new byte[2] : new byte[2] { 255, 255 }).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await WriteTagAsync(address, 193, value.Select((bool m) => (byte)(m ? 1u : 0u)).ToArray(), (!HslHelper.IsAddressEndWithIndex(address)) ? 1 : value.Length).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<OperateResult> WriteAsync(string address, byte value)
	{
		return await WriteTagAsync(address, 194, new byte[2] { value, 0 }).ConfigureAwait(continueOnCapturedContext: false);
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
		return await AllenBradleyHelper.ReadDateAsync(this, address).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult> WriteDateAsync(string address, DateTime date)
	{
		return await AllenBradleyHelper.WriteDateAsync(this, address, date).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult> WriteTimeAndDateAsync(string address, DateTime date)
	{
		return await AllenBradleyHelper.WriteTimeAndDateAsync(this, address, date).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<TimeSpan>> ReadTimeAsync(string address)
	{
		return await AllenBradleyHelper.ReadTimeAsync(this, address).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult> WriteTimeAsync(string address, TimeSpan time)
	{
		return await AllenBradleyHelper.WriteTimeAsync(this, address, time).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult> WriteTimeOfDateAsync(string address, TimeSpan timeOfDate)
	{
		return await AllenBradleyHelper.WriteTimeOfDateAsync(this, address, timeOfDate).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected virtual byte[] PackCommandService(byte[] portSlot, params byte[][] cips)
	{
		if (MessageRouter != null)
		{
			portSlot = MessageRouter.GetRouter();
		}
		return AllenBradleyHelper.PackCommandService(portSlot, cips);
	}

	protected virtual int GetWriteValueLength(string address, int length)
	{
		return length;
	}

	public override string ToString()
	{
		return $"AllenBradleyNet[{IpAddress}:{Port}]";
	}
}
