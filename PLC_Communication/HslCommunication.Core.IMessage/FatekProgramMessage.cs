using System.IO;
using HslCommunication.Profinet.FATEK.Helper;

namespace HslCommunication.Core.IMessage;

public class FatekProgramMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => -1;

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return FatekProgramHelper.CheckReceiveDataComplete(ms);
	}
}
