using System;

namespace HslCommunication.Core.IMessage;

public class MemobusMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 12;

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes?.Length >= ProtocolHeadBytesLength)
		{
			int num = BitConverter.ToUInt16(base.HeadBytes, 6) - 12;
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}
		return 0;
	}
}
