using System;
using HslCommunication.Core.Address;
using HslCommunication.Profinet.Melsec;

namespace HslCommunication.Profinet.Panasonic;

public class PanasonicMcNet : MelsecMcNet
{
	public PanasonicMcNet()
	{
	}

	public PanasonicMcNet(string ipAddress, int port)
		: base(ipAddress, port)
	{
	}

	public override OperateResult<McAddressData> McAnalysisAddress(string address, ushort length, bool isBit)
	{
		return McAddressData.ParsePanasonicFrom(address, length, isBit);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		ushort num = BitConverter.ToUInt16(response, 9);
		if (num != 0)
		{
			return new OperateResult<byte[]>(num, PanasonicHelper.GetMcErrorDescription(num));
		}
		return OperateResult.CreateSuccessResult(response.RemoveBegin(11));
	}

	public override string ToString()
	{
		return $"PanasonicMcNet[{IpAddress}:{Port}]";
	}
}
