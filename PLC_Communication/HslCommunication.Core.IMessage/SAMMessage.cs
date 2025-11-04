namespace HslCommunication.Core.IMessage;

public class SAMMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 7;

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return true;
		}
		return base.HeadBytes[0] == 170 && base.HeadBytes[1] == 170 && base.HeadBytes[2] == 170 && base.HeadBytes[3] == 150 && base.HeadBytes[4] == 105;
	}

	public int GetContentLengthByHeadBytes()
	{
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 7)
		{
			return base.HeadBytes[5] * 256 + base.HeadBytes[6];
		}
		return 0;
	}
}
