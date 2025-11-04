using System;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core.IMessage;

public class HslMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 32;

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return false;
		}
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 32)
		{
			return SoftBasic.IsTwoBytesEquel(base.HeadBytes, 12, token, 0, 16);
		}
		return false;
	}

	public int GetContentLengthByHeadBytes()
	{
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 32)
		{
			return BitConverter.ToInt32(base.HeadBytes, 28);
		}
		return 0;
	}

	public override int GetHeadBytesIdentity()
	{
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 32)
		{
			return BitConverter.ToInt32(base.HeadBytes, 4);
		}
		return 0;
	}
}
