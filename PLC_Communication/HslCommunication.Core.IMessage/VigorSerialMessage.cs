using System.IO;
using HslCommunication.Profinet.Vigor.Helper;

namespace HslCommunication.Core.IMessage;

public class VigorSerialMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => -1;

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		byte[] array = ms.ToArray();
		return VigorVsHelper.CheckReceiveDataComplete(array, array.Length);
	}
}
