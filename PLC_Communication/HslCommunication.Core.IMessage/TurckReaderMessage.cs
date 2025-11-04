namespace HslCommunication.Core.IMessage;

public class TurckReaderMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 3;

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return true;
		}
		return base.HeadBytes[0] == 170;
	}

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes[2] <= 3)
		{
			return 0;
		}
		int num = base.HeadBytes[2] - 3;
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public override int CheckMessageMatch(byte[] send, byte[] receive)
	{
		if (CheckResponseACK(receive))
		{
			return -1;
		}
		return 1;
	}

	private static bool CheckResponseACK(byte[] content)
	{
		try
		{
			if (content[1] == 7 && content[2] == 7 && (content[3] == 104 || (content[3] == 105 && content[4] == 137) || content[3] == 112 || (content[3] == 105 && content[4] == 129)))
			{
				return true;
			}
		}
		catch
		{
			return false;
		}
		return false;
	}
}
