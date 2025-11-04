using System;

namespace HslCommunication.Core.IMessage;

public class FujiSPHMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 20;

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes == null)
		{
			return 0;
		}
		return BitConverter.ToUInt16(base.HeadBytes, 18);
	}
}
