using System;
using System.Text;

namespace HslCommunication.Core.IMessage;

public class MelsecA1EAsciiMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 4;

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes[2] == 53 && base.HeadBytes[3] == 66)
		{
			return 4;
		}
		if (base.HeadBytes[2] == 48 && base.HeadBytes[3] == 48)
		{
			int num = Convert.ToInt32(Encoding.ASCII.GetString(base.SendBytes, 20, 2), 16);
			if (num == 0)
			{
				num = 256;
			}
			switch (base.HeadBytes[1])
			{
			case 48:
				return (num % 2 == 1) ? (num + 1) : num;
			case 49:
				return num * 4;
			case 50:
			case 51:
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
			return base.HeadBytes[0] - base.SendBytes[0] == 8;
		}
		return false;
	}
}
