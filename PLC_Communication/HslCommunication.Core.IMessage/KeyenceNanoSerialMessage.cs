using System.IO;

namespace HslCommunication.Core.IMessage;

public class KeyenceNanoSerialMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => -1;

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		byte[] array = ms.ToArray();
		if (array.Length > 2)
		{
			return array[array.Length - 2] == 13 && array[array.Length - 1] == 10;
		}
		return true;
	}
}
