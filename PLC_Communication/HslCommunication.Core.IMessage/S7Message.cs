namespace HslCommunication.Core.IMessage;

public class S7Message : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 4;

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return false;
		}
		if (base.HeadBytes[0] == 3 && base.HeadBytes[1] == 0)
		{
			return true;
		}
		return false;
	}

	public int GetContentLengthByHeadBytes()
	{
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 4)
		{
			int num = base.HeadBytes[2] * 256 + base.HeadBytes[3] - 4;
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}
		return 0;
	}

	public override int CheckMessageMatch(byte[] send, byte[] receive)
	{
		if (receive != null && receive.Length >= 14 && send != null && send.Length >= 14 && receive[5] == 240)
		{
			if (send[11] == receive[11] && send[12] == receive[12])
			{
				return 1;
			}
			return -1;
		}
		return base.CheckMessageMatch(send, receive);
	}
}
