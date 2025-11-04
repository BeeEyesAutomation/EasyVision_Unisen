using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Pipe;
using HslCommunication.Profinet.Beckhoff.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.Beckhoff;

public class BeckhoffAdsNet : DeviceTcpNet
{
	private byte[] targetAMSNetId = new byte[8];

	private byte[] sourceAMSNetId = new byte[8];

	private string senderAMSNetId = string.Empty;

	private string _targetAmsNetID = string.Empty;

	private bool useAutoAmsNetID = false;

	private bool useTagCache = false;

	private readonly Dictionary<string, uint> tagCaches = new Dictionary<string, uint>();

	private readonly object tagLock = new object();

	private readonly SoftIncrementCount incrementCount = new SoftIncrementCount(2147483647L, 1L);

	[HslMqttApi(HttpMethod = "GET", Description = "Get or set the IP address of the remote server. If it is a local test, then it needs to be set to 127.0.0.1")]
	public override string IpAddress
	{
		get
		{
			return base.IpAddress;
		}
		set
		{
			base.IpAddress = value;
			string[] array = base.IpAddress.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				targetAMSNetId[i] = byte.Parse(array[i]);
			}
		}
	}

	public bool UseTagCache
	{
		get
		{
			return useTagCache;
		}
		set
		{
			useTagCache = value;
		}
	}

	public bool UseAutoAmsNetID
	{
		get
		{
			return useAutoAmsNetID;
		}
		set
		{
			useAutoAmsNetID = value;
		}
	}

	public int AmsPort
	{
		get
		{
			return BitConverter.ToUInt16(targetAMSNetId, 6);
		}
		set
		{
			targetAMSNetId[6] = BitConverter.GetBytes(value)[0];
			targetAMSNetId[7] = BitConverter.GetBytes(value)[1];
		}
	}

	public BeckhoffAdsNet()
	{
		base.WordLength = 2;
		targetAMSNetId[4] = 1;
		targetAMSNetId[5] = 1;
		targetAMSNetId[6] = 83;
		targetAMSNetId[7] = 3;
		sourceAMSNetId[4] = 1;
		sourceAMSNetId[5] = 1;
		base.ByteTransform = new RegularByteTransform();
		CommunicationPipe.UseServerActivePush = true;
	}

	public BeckhoffAdsNet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new AdsNetMessage();
	}

	public void SetTargetAMSNetId(string amsNetId)
	{
		if (!string.IsNullOrEmpty(amsNetId))
		{
			AdsHelper.StrToAMSNetId(amsNetId).CopyTo(targetAMSNetId, 0);
			_targetAmsNetID = amsNetId;
		}
	}

	public void SetSenderAMSNetId(string amsNetId)
	{
		if (!string.IsNullOrEmpty(amsNetId))
		{
			AdsHelper.StrToAMSNetId(amsNetId).CopyTo(sourceAMSNetId, 0);
			senderAMSNetId = amsNetId;
		}
	}

	public string GetSenderAMSNetId()
	{
		return AdsHelper.GetAmsNetIdString(sourceAMSNetId, 0);
	}

	public string GetTargetAMSNetId()
	{
		return AdsHelper.GetAmsNetIdString(targetAMSNetId, 0);
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		uint value = (uint)incrementCount.GetCurrentValue();
		targetAMSNetId.CopyTo(command, 6);
		sourceAMSNetId.CopyTo(command, 14);
		command[34] = BitConverter.GetBytes(value)[0];
		command[35] = BitConverter.GetBytes(value)[1];
		command[36] = BitConverter.GetBytes(value)[2];
		command[37] = BitConverter.GetBytes(value)[3];
		return base.PackCommandWithHeader(command);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		if (response.Length >= 38)
		{
			ushort num = base.ByteTransform.TransUInt16(response, 22);
			OperateResult operateResult = AdsHelper.CheckResponse(response);
			if (!operateResult.IsSuccess)
			{
				if (operateResult.ErrorCode == 1809 && (num == 2 || num == 3))
				{
					lock (tagLock)
					{
						tagCaches.Clear();
					}
				}
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			try
			{
				switch (num)
				{
				case 1:
					return OperateResult.CreateSuccessResult(response.RemoveBegin(42));
				case 2:
					return OperateResult.CreateSuccessResult(response.RemoveBegin(46));
				case 3:
					return OperateResult.CreateSuccessResult(new byte[0]);
				case 4:
					return OperateResult.CreateSuccessResult(response.RemoveBegin(42));
				case 5:
					return OperateResult.CreateSuccessResult(response.RemoveBegin(42));
				case 6:
					return OperateResult.CreateSuccessResult(response.RemoveBegin(42));
				case 7:
					return OperateResult.CreateSuccessResult(new byte[0]);
				case 9:
					return OperateResult.CreateSuccessResult(response.RemoveBegin(46));
				case 8:
					break;
				}
			}
			catch (Exception ex)
			{
				return new OperateResult<byte[]>("UnpackResponseContent failed: " + ex.Message + Environment.NewLine + "Source: " + response.ToHexString(' '));
			}
		}
		return base.UnpackResponseContent(send, response);
	}

	protected override void ExtraAfterReadFromCoreServer(OperateResult read)
	{
		if (!read.IsSuccess && read.ErrorCode < 0 && useTagCache)
		{
			lock (tagLock)
			{
				tagCaches.Clear();
			}
		}
		base.ExtraAfterReadFromCoreServer(read);
	}

	protected override OperateResult InitializationOnConnect()
	{
		if (string.IsNullOrEmpty(senderAMSNetId) && string.IsNullOrEmpty(_targetAmsNetID))
		{
			useAutoAmsNetID = true;
		}
		if (useAutoAmsNetID)
		{
			OperateResult<byte[]> localNetId = GetLocalNetId();
			if (!localNetId.IsSuccess)
			{
				return localNetId;
			}
			if (localNetId.Content.Length >= 12)
			{
				Array.Copy(localNetId.Content, 6, targetAMSNetId, 0, 6);
			}
			OperateResult operateResult = CommunicationPipe.Send(AdsHelper.PackAmsTcpHelper(AmsTcpHeaderFlags.PortConnect, new byte[2]));
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			OperateResult<byte[]> operateResult2 = CommunicationPipe.ReceiveMessage(GetNewNetMessage(), null, useActivePush: false);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			if (operateResult2.Content.Length >= 14)
			{
				Array.Copy(operateResult2.Content, 6, sourceAMSNetId, 0, 8);
			}
		}
		else if (string.IsNullOrEmpty(senderAMSNetId) && CommunicationPipe is PipeTcpNet pipeTcpNet)
		{
			IPEndPoint iPEndPoint = (IPEndPoint)pipeTcpNet.Socket.LocalEndPoint;
			sourceAMSNetId[6] = BitConverter.GetBytes(iPEndPoint.Port)[0];
			sourceAMSNetId[7] = BitConverter.GetBytes(iPEndPoint.Port)[1];
			iPEndPoint.Address.GetAddressBytes().CopyTo(sourceAMSNetId, 0);
		}
		if (useTagCache)
		{
			lock (tagLock)
			{
				tagCaches.Clear();
			}
		}
		CommunicationPipe.UseServerActivePush = true;
		return base.InitializationOnConnect();
	}

	private OperateResult<byte[]> GetLocalNetId()
	{
		PipeTcpNet pipeTcpNet = null;
		pipeTcpNet = ((!(CommunicationPipe.GetType() == typeof(PipeSslNet))) ? new PipeTcpNet(IpAddress, Port)
		{
			ConnectTimeOut = ConnectTimeOut,
			ReceiveTimeOut = base.ReceiveTimeOut
		} : new PipeSslNet(IpAddress, Port, serverMode: false)
		{
			ConnectTimeOut = ConnectTimeOut,
			ReceiveTimeOut = base.ReceiveTimeOut
		});
		OperateResult<bool> operateResult = pipeTcpNet.OpenCommunication();
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		OperateResult operateResult2 = pipeTcpNet.Send(AdsHelper.PackAmsTcpHelper(AmsTcpHeaderFlags.GetLocalNetId, new byte[4]));
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = pipeTcpNet.ReceiveMessage(GetNewNetMessage(), null);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		pipeTcpNet.CloseCommunication();
		return operateResult3;
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		if (string.IsNullOrEmpty(senderAMSNetId) && string.IsNullOrEmpty(_targetAmsNetID))
		{
			useAutoAmsNetID = true;
		}
		if (useAutoAmsNetID)
		{
			OperateResult<byte[]> read1 = await GetLocalNetIdAsync();
			if (!read1.IsSuccess)
			{
				return read1;
			}
			if (read1.Content.Length >= 12)
			{
				Array.Copy(read1.Content, 6, targetAMSNetId, 0, 6);
			}
			OperateResult send2 = await CommunicationPipe.SendAsync(AdsHelper.PackAmsTcpHelper(AmsTcpHeaderFlags.PortConnect, new byte[2])).ConfigureAwait(continueOnCapturedContext: false);
			if (!send2.IsSuccess)
			{
				return send2;
			}
			OperateResult<byte[]> read2 = await CommunicationPipe.ReceiveMessageAsync(GetNewNetMessage(), null, useActivePush: false).ConfigureAwait(continueOnCapturedContext: false);
			if (!read2.IsSuccess)
			{
				return read2;
			}
			if (read2.Content.Length >= 14)
			{
				Array.Copy(read2.Content, 6, sourceAMSNetId, 0, 8);
			}
		}
		else if (string.IsNullOrEmpty(senderAMSNetId))
		{
			CommunicationPipe communicationPipe = CommunicationPipe;
			if (communicationPipe is PipeTcpNet pipe)
			{
				IPEndPoint iPEndPoint = (IPEndPoint)pipe.Socket.LocalEndPoint;
				sourceAMSNetId[6] = BitConverter.GetBytes(iPEndPoint.Port)[0];
				sourceAMSNetId[7] = BitConverter.GetBytes(iPEndPoint.Port)[1];
				iPEndPoint.Address.GetAddressBytes().CopyTo(sourceAMSNetId, 0);
			}
		}
		if (useTagCache)
		{
			lock (tagLock)
			{
				tagCaches.Clear();
			}
		}
		CommunicationPipe.UseServerActivePush = true;
		return await base.InitializationOnConnectAsync();
	}

	private async Task<OperateResult<byte[]>> GetLocalNetIdAsync()
	{
		PipeTcpNet pipeTcpNet = ((!(CommunicationPipe.GetType() == typeof(PipeSslNet))) ? new PipeTcpNet(IpAddress, Port)
		{
			ConnectTimeOut = ConnectTimeOut,
			ReceiveTimeOut = base.ReceiveTimeOut
		} : new PipeSslNet(IpAddress, Port, serverMode: false)
		{
			ConnectTimeOut = ConnectTimeOut,
			ReceiveTimeOut = base.ReceiveTimeOut
		});
		OperateResult<bool> opSocket = await pipeTcpNet.OpenCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (!opSocket.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(opSocket);
		}
		OperateResult send = await pipeTcpNet.SendAsync(AdsHelper.PackAmsTcpHelper(AmsTcpHeaderFlags.GetLocalNetId, new byte[4])).ConfigureAwait(continueOnCapturedContext: false);
		if (!send.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(send);
		}
		OperateResult<byte[]> read = await pipeTcpNet.ReceiveMessageAsync(GetNewNetMessage(), null).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return read;
		}
		await pipeTcpNet.CloseCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
		return read;
	}

	protected override bool DecideWhetherQAMessage(CommunicationPipe pipe, OperateResult<byte[]> receive)
	{
		if (!receive.IsSuccess)
		{
			if (useTagCache)
			{
				lock (tagLock)
				{
					tagCaches.Clear();
				}
			}
			return false;
		}
		byte[] content = receive.Content;
		if (content.Length >= 2 && BitConverter.ToUInt16(content, 0) == 0)
		{
			if (content.Length >= 24)
			{
				ushort num = base.ByteTransform.TransUInt16(content, 22);
				if (num == 8)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public OperateResult<uint> ReadValueHandle(string address)
	{
		if (!address.StartsWith("s="))
		{
			return new OperateResult<uint>("When read valueHandle, address must startwith 's=', forexample: s=MAIN.A");
		}
		OperateResult<byte[]> operateResult = AdsHelper.BuildReadWriteCommand(address, 4, isBit: false, AdsHelper.StrToAdsBytes(address.Substring(2)));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<uint>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<uint>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToUInt32(operateResult2.Content, 0));
	}

	public OperateResult<string> TransValueHandle(string address)
	{
		if (address.StartsWith("s=") || address.StartsWith("S="))
		{
			if (useTagCache)
			{
				lock (tagLock)
				{
					if (tagCaches.ContainsKey(address))
					{
						return OperateResult.CreateSuccessResult($"i={tagCaches[address]}");
					}
				}
			}
			OperateResult<uint> operateResult = ReadValueHandle(address);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult);
			}
			if (useTagCache)
			{
				lock (tagLock)
				{
					if (!tagCaches.ContainsKey(address))
					{
						tagCaches.Add(address, operateResult.Content);
					}
				}
			}
			return OperateResult.CreateSuccessResult($"i={operateResult.Content}");
		}
		return OperateResult.CreateSuccessResult(address);
	}

	[HslMqttApi("ReadAdsDeviceInfo", "读取Ads设备的设备信息。主要是版本号，设备名称")]
	public OperateResult<AdsDeviceInfo> ReadAdsDeviceInfo()
	{
		OperateResult<byte[]> operateResult = AdsHelper.BuildReadDeviceInfoCommand();
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<AdsDeviceInfo>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<AdsDeviceInfo>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(new AdsDeviceInfo(operateResult2.Content));
	}

	[HslMqttApi("ReadAdsState", "读取Ads设备的状态信息")]
	public OperateResult<ushort, ushort> ReadAdsState()
	{
		OperateResult<byte[]> operateResult = AdsHelper.BuildReadStateCommand();
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort, ushort>(operateResult);
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort, ushort>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToUInt16(operateResult2.Content, 0), BitConverter.ToUInt16(operateResult2.Content, 2));
	}

	[HslMqttApi("WriteAdsState", "写入Ads的状态，可以携带数据信息，数据可以为空")]
	public OperateResult WriteAdsState(short state, short deviceState, byte[] data)
	{
		OperateResult<byte[]> operateResult = AdsHelper.BuildWriteControlCommand(state, deviceState, data);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	public OperateResult ReleaseSystemHandle(uint handle)
	{
		OperateResult<byte[]> operateResult = AdsHelper.BuildReleaseSystemHandle(handle);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return ReadFromCoreServer(operateResult.Content);
	}

	public async Task<OperateResult<uint>> ReadValueHandleAsync(string address)
	{
		if (!address.StartsWith("s="))
		{
			return new OperateResult<uint>("When read valueHandle, address must startwith 's=', forexample: s=MAIN.A");
		}
		OperateResult<byte[]> build = AdsHelper.BuildReadWriteCommand(address, 4, isBit: false, AdsHelper.StrToAdsBytes(address.Substring(2)));
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<uint>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<uint>(read);
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToUInt32(read.Content, 0));
	}

	public async Task<OperateResult<string>> TransValueHandleAsync(string address)
	{
		if (address.StartsWith("s=") || address.StartsWith("S="))
		{
			if (useTagCache)
			{
				lock (tagLock)
				{
					if (tagCaches.ContainsKey(address))
					{
						return OperateResult.CreateSuccessResult($"i={tagCaches[address]}");
					}
				}
			}
			OperateResult<uint> read = await ReadValueHandleAsync(address);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(read);
			}
			if (useTagCache)
			{
				lock (tagLock)
				{
					if (!tagCaches.ContainsKey(address))
					{
						tagCaches.Add(address, read.Content);
					}
				}
			}
			return OperateResult.CreateSuccessResult($"i={read.Content}");
		}
		return OperateResult.CreateSuccessResult(address);
	}

	public async Task<OperateResult<AdsDeviceInfo>> ReadAdsDeviceInfoAsync()
	{
		OperateResult<byte[]> build = AdsHelper.BuildReadDeviceInfoCommand();
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<AdsDeviceInfo>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<AdsDeviceInfo>(read);
		}
		return OperateResult.CreateSuccessResult(new AdsDeviceInfo(read.Content));
	}

	public async Task<OperateResult<ushort, ushort>> ReadAdsStateAsync()
	{
		OperateResult<byte[]> build = AdsHelper.BuildReadStateCommand();
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort, ushort>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<ushort, ushort>(read);
		}
		return OperateResult.CreateSuccessResult(BitConverter.ToUInt16(read.Content, 0), BitConverter.ToUInt16(read.Content, 2));
	}

	public async Task<OperateResult> WriteAdsStateAsync(short state, short deviceState, byte[] data)
	{
		OperateResult<byte[]> build = AdsHelper.BuildWriteControlCommand(state, deviceState, data);
		if (!build.IsSuccess)
		{
			return build;
		}
		return await ReadFromCoreServerAsync(build.Content);
	}

	public async Task<OperateResult> ReleaseSystemHandleAsync(uint handle)
	{
		OperateResult<byte[]> build = AdsHelper.BuildReleaseSystemHandle(handle);
		if (!build.IsSuccess)
		{
			return build;
		}
		return await ReadFromCoreServerAsync(build.Content);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		OperateResult<string> operateResult = TransValueHandle(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		address = operateResult.Content;
		OperateResult<byte[]> operateResult2 = AdsHelper.BuildReadCommand(address, length, isBit: false);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return ReadFromCoreServer(operateResult2.Content);
	}

	private void AddLengthAndOffset(List<ushort> length, List<int> offset, ref int index, int len)
	{
		length.Add((ushort)len);
		offset.Add(index);
		index += len;
	}

	public override OperateResult<T> Read<T>()
	{
		Type typeFromHandle = typeof(T);
		object obj = typeFromHandle.Assembly.CreateInstance(typeFromHandle.FullName);
		List<HslAddressProperty> hslPropertyInfos = HslReflectionHelper.GetHslPropertyInfos(typeFromHandle, GetType(), null, base.ByteTransform);
		string[] address = hslPropertyInfos.Select((HslAddressProperty m) => m.DeviceAddressAttribute.Address).ToArray();
		ushort[] length = hslPropertyInfos.Select((HslAddressProperty m) => (ushort)m.ByteLength).ToArray();
		OperateResult<byte[]> operateResult = Read(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(operateResult);
		}
		HslReflectionHelper.SetPropertyValueFrom(base.ByteTransform, obj, hslPropertyInfos, operateResult.Content);
		return OperateResult.CreateSuccessResult((T)obj);
	}

	public OperateResult<T> ReadStruct<T>(string address) where T : struct
	{
		T structure = new T();
		OperateResult<byte[]> operateResult = Read(address, (ushort)Marshal.SizeOf(structure));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(operateResult);
		}
		GCHandle gCHandle = GCHandle.Alloc(operateResult.Content, GCHandleType.Pinned);
		structure = (T)Marshal.PtrToStructure(gCHandle.AddrOfPinnedObject(), typeof(T));
		gCHandle.Free();
		return OperateResult.CreateSuccessResult(structure);
	}

	public OperateResult WriteStruct<T>(string address, T value) where T : struct
	{
		byte[] value2 = new byte[Marshal.SizeOf(value)];
		GCHandle gCHandle = GCHandle.Alloc(value2, GCHandleType.Pinned);
		Marshal.StructureToPtr(value, gCHandle.AddrOfPinnedObject(), fDeleteOld: false);
		gCHandle.Free();
		return Write(address, value2);
	}

	[HslMqttApi("ReadBatchByte", "To read PLC data in batches, you need to pass in the address array and the read length array information.")]
	public OperateResult<byte[]> Read(string[] address, ushort[] length)
	{
		if (address.Length != length.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		for (int i = 0; i < address.Length; i++)
		{
			OperateResult<string> operateResult = TransValueHandle(address[i]);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(operateResult);
			}
			address[i] = operateResult.Content;
		}
		OperateResult<byte[]> operateResult2 = AdsHelper.BuildReadCommand(address, length);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return ReadFromCoreServer(operateResult2.Content);
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		OperateResult<string> operateResult = TransValueHandle(address);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		address = operateResult.Content;
		OperateResult<byte[]> operateResult2 = AdsHelper.BuildWriteCommand(address, value, isBit: false);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return ReadFromCoreServer(operateResult2.Content);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		if (Regex.IsMatch(address, "^[MIQ][0-9]+\\.[0-7]$", RegexOptions.IgnoreCase) && length > 1)
		{
			return HslHelper.ReadBool(this, address, length, 8);
		}
		OperateResult<string> operateResult = TransValueHandle(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		address = operateResult.Content;
		OperateResult<byte[]> operateResult2 = AdsHelper.BuildReadCommand(address, length, isBit: true);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult2);
		}
		OperateResult<byte[]> operateResult3 = ReadFromCoreServer(operateResult2.Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult3);
		}
		return OperateResult.CreateSuccessResult(operateResult3.Content.Select((byte m) => m != 0).ToArray());
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		OperateResult<string> operateResult = TransValueHandle(address);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		address = operateResult.Content;
		OperateResult<byte[]> operateResult2 = AdsHelper.BuildWriteCommand(address, value, isBit: true);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return ReadFromCoreServer(operateResult2.Content);
	}

	[HslMqttApi("ReadByte", "")]
	public OperateResult<byte> ReadByte(string address)
	{
		return ByteTransformHelper.GetResultFromArray(Read(address, 1));
	}

	[HslMqttApi("WriteByte", "")]
	public OperateResult Write(string address, byte value)
	{
		return Write(address, new byte[1] { value });
	}

	public override async Task<OperateResult<T>> ReadAsync<T>()
	{
		Type type = typeof(T);
		object obj = type.Assembly.CreateInstance(type.FullName);
		List<HslAddressProperty> array = HslReflectionHelper.GetHslPropertyInfos(type, GetType(), null, base.ByteTransform);
		string[] address = array.Select((HslAddressProperty m) => m.DeviceAddressAttribute.Address).ToArray();
		ushort[] length = array.Select((HslAddressProperty m) => (ushort)m.ByteLength).ToArray();
		OperateResult<byte[]> read = await ReadAsync(address, length).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(read);
		}
		HslReflectionHelper.SetPropertyValueFrom(base.ByteTransform, obj, array, read.Content);
		return OperateResult.CreateSuccessResult((T)obj);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<string> addressCheck = await TransValueHandleAsync(address);
		if (!addressCheck.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(addressCheck);
		}
		address = addressCheck.Content;
		OperateResult<byte[]> build = AdsHelper.BuildReadCommand(address, length, isBit: false);
		if (!build.IsSuccess)
		{
			return build;
		}
		return await ReadFromCoreServerAsync(build.Content).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<byte[]>> ReadAsync(string[] address, ushort[] length)
	{
		if (address.Length != length.Length)
		{
			return new OperateResult<byte[]>(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		for (int i = 0; i < address.Length; i++)
		{
			OperateResult<string> addressCheck = TransValueHandle(address[i]);
			if (!addressCheck.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte[]>(addressCheck);
			}
			address[i] = addressCheck.Content;
		}
		OperateResult<byte[]> build = AdsHelper.BuildReadCommand(address, length);
		if (!build.IsSuccess)
		{
			return build;
		}
		return await ReadFromCoreServerAsync(build.Content).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<string> addressCheck = await TransValueHandleAsync(address);
		if (!addressCheck.IsSuccess)
		{
			return addressCheck;
		}
		address = addressCheck.Content;
		OperateResult<byte[]> build = AdsHelper.BuildWriteCommand(address, value, isBit: false);
		if (!build.IsSuccess)
		{
			return build;
		}
		return await ReadFromCoreServerAsync(build.Content).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		if (Regex.IsMatch(address, "^[MIQ][0-9]+\\.[0-7]$", RegexOptions.IgnoreCase) && length > 1)
		{
			return await HslHelper.ReadBoolAsync(this, address, length, 8).ConfigureAwait(continueOnCapturedContext: false);
		}
		OperateResult<string> addressCheck = await TransValueHandleAsync(address).ConfigureAwait(continueOnCapturedContext: false);
		if (!addressCheck.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(addressCheck);
		}
		address = addressCheck.Content;
		OperateResult<byte[]> build = AdsHelper.BuildReadCommand(address, length, isBit: true);
		if (!build.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(build);
		}
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(build.Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.Select((byte m) => m != 0).ToArray());
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		OperateResult<string> addressCheck = await TransValueHandleAsync(address).ConfigureAwait(continueOnCapturedContext: false);
		if (!addressCheck.IsSuccess)
		{
			return addressCheck;
		}
		address = addressCheck.Content;
		OperateResult<byte[]> build = AdsHelper.BuildWriteCommand(address, value, isBit: true);
		if (!build.IsSuccess)
		{
			return build;
		}
		return await ReadFromCoreServerAsync(build.Content).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadAsync(address, 1).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<OperateResult> WriteAsync(string address, byte value)
	{
		return await WriteAsync(address, new byte[1] { value }).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override string ToString()
	{
		return $"BeckhoffAdsNet[{IpAddress}:{Port}]";
	}
}
