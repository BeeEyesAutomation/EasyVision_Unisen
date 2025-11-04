namespace HslCommunication.Core.IMessage;

public class MelsecA1EBinaryMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 2;

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes[1] == 91)
		{
			return 2;
		}
		if (base.HeadBytes[1] == 0)
		{
			switch (base.HeadBytes[0])
			{
			case 128:
				return (base.SendBytes[10] != 0) ? ((base.SendBytes[10] + 1) / 2) : 128;
			case 129:
				return base.SendBytes[10] * 2;
			case 130:
			case 131:
				return 0;
			default:
				return 0;
			}
		}
		return 0;
	}

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes != null)
		{
			return base.HeadBytes[0] - base.SendBytes[0] == 128;
		}
		return false;
	}
}
