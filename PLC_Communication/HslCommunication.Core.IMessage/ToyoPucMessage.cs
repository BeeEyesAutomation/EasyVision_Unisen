using System;

namespace HslCommunication.Core.IMessage;

public class ToyoPucMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 4;

	public int GetContentLengthByHeadBytes()
	{
		try
		{
			byte[] headBytes = base.HeadBytes;
			if (headBytes != null && headBytes.Length >= 4)
			{
				return BitConverter.ToUInt16(base.HeadBytes, 2);
			}
			return 0;
		}
		catch
		{
			return 0;
		}
	}
}
