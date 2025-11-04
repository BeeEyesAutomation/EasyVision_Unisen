using System;

namespace HslCommunication.Core.IMessage;

public class EFORTMessagePrevious : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 17;

	public int GetContentLengthByHeadBytes()
	{
		int num = BitConverter.ToInt16(base.HeadBytes, 15) - 17;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}
}
