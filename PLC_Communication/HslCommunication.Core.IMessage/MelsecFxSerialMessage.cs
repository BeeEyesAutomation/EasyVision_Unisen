using System.IO;
using HslCommunication.Profinet.Melsec.Helper;

namespace HslCommunication.Core.IMessage;

public class MelsecFxSerialMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => -1;

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return MelsecFxSerialHelper.CheckReceiveDataComplete(ms.ToArray());
	}
}
