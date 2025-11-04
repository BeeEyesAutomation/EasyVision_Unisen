using System;

namespace HslCommunication.Core.IMessage;

public class LsisFastEnetMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 20;

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return false;
		}
		return base.HeadBytes[0] == 76;
	}

	public int GetContentLengthByHeadBytes()
	{
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 20)
		{
			return BitConverter.ToUInt16(base.HeadBytes, 16);
		}
		return 0;
	}

	public override int GetHeadBytesIdentity()
	{
		return BitConverter.ToUInt16(base.HeadBytes, 14);
	}
}
