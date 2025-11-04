using System;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Instrument.DLT.Helper;

namespace HslCommunication.Instrument.DLT;

public class DLT698TcpNet : DLT698OverTcp, IDlt698, IReadWriteDevice, IReadWriteNet
{
	public DLT698TcpNet()
	{
	}

	public DLT698TcpNet(string station = "AAAAAAAAAAAA")
		: base(station)
	{
	}

	public DLT698TcpNet(string ipAddress, int port, string station = "AAAAAAAAAAAA")
		: base(ipAddress, port, station)
	{
	}

	protected override OperateResult InitializationOnConnect()
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(CommunicationPipe, DLT698Helper.BuildEntireCommand(129, base.Station, base.CA, CreateLoginApdu(1, 0, 0)).Content, hasResponseData: true, usePackAndUnpack: true);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = ReadFromCoreServer(CommunicationPipe, DLT698Helper.BuildEntireCommand(129, base.Station, base.CA, CreateConnectApdu()).Content, hasResponseData: true, usePackAndUnpack: true);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return base.InitializationOnConnect();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		OperateResult<byte[]> read1 = await ReadFromCoreServerAsync(CommunicationPipe, DLT698Helper.BuildEntireCommand(129, base.Station, base.CA, CreateLoginApdu(1, 0, 0)).Content, hasResponseData: true, usePackAndUnpack: true);
		if (!read1.IsSuccess)
		{
			return read1;
		}
		OperateResult<byte[]> read2 = await ReadFromCoreServerAsync(CommunicationPipe, DLT698Helper.BuildEntireCommand(129, base.Station, base.CA, CreateConnectApdu()).Content, hasResponseData: true, usePackAndUnpack: true);
		if (!read2.IsSuccess)
		{
			return read2;
		}
		return await base.InitializationOnConnectAsync();
	}

	public override string ToString()
	{
		return $"DLT698TcpNet[{IpAddress}:{Port}]";
	}

	internal static byte[] CreateLoginApdu(byte services = 1, byte piid = 0, byte type = 0)
	{
		byte[] array = new byte[15]
		{
			services, piid, type, 0, 132, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0
		};
		DLT698Helper.SetDltDataTime(array, 5, DateTime.Now);
		return array;
	}

	internal static byte[] CreateConnectApdu()
	{
		return "02 00 00 10 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF 04 00 04 00 01 04 00 00 00 00 64 00 00".ToHexBytes();
	}
}
