using System;

namespace HslCommunication.Core.IMessage;

public class MelsecQnA3EBinaryMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 9;

	public int GetContentLengthByHeadBytes()
	{
		return BitConverter.ToUInt16(base.HeadBytes, 7);
	}

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return false;
		}
		if (base.HeadBytes[0] == 208 && base.HeadBytes[1] == 0)
		{
			return true;
		}
		return false;
	}
}
