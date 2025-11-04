using System;

namespace HslCommunication.Core.IMessage;

public class AdsNetMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 6;

	public int GetContentLengthByHeadBytes()
	{
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 6)
		{
			int num = BitConverter.ToInt32(base.HeadBytes, 2);
			if (num > 100000000)
			{
				num = 100000000;
			}
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}
		return 0;
	}
}
