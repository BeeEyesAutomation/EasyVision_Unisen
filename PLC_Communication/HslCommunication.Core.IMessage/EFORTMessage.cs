using System;

namespace HslCommunication.Core.IMessage;

public class EFORTMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 18;

	public int GetContentLengthByHeadBytes()
	{
		int num = BitConverter.ToInt16(base.HeadBytes, 16) - 18;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}
}
