using System.IO;
using HslCommunication.Core.IMessage;

namespace HslCommunication.Profinet.Special;

internal class EuFunMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => -1;

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return EcFanMachine.CheckReceiveDataComplete(send, ms.ToArray());
	}
}
