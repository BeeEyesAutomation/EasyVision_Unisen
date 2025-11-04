using HslCommunication.Core;
using HslCommunication.Core.IMessage;
using HslCommunication.Profinet.Melsec.Helper;

namespace HslCommunication.Profinet.Melsec;

public class MelsecMcAsciiNet : MelsecMcNet
{
	public override McType McType => McType.MCAscii;

	public MelsecMcAsciiNet()
	{
		base.WordLength = 1;
		LogMsgFormatBinary = false;
		base.ByteTransform = new RegularByteTransform();
	}

	public MelsecMcAsciiNet(string ipAddress, int port)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new MelsecQnA3EAsciiMessage();
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		return McAsciiHelper.PackMcCommand(this, command);
	}

	public override OperateResult<byte[]> UnpackResponseContent(byte[] send, byte[] response)
	{
		OperateResult operateResult = McAsciiHelper.CheckResponseContent(response);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(response.RemoveBegin(22));
	}

	public override byte[] ExtractActualData(byte[] response, bool isBit)
	{
		return McAsciiHelper.ExtractActualDataHelper(response, isBit);
	}

	public override string ToString()
	{
		return $"MelsecMcAsciiNet[{IpAddress}:{Port}]";
	}
}
