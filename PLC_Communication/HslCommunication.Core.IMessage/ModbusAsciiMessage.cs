using System.IO;
using HslCommunication.ModBus;

namespace HslCommunication.Core.IMessage;

public class ModbusAsciiMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => -1;

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return ModbusInfo.CheckAsciiReceiveDataComplete(ms.ToArray());
	}
}
