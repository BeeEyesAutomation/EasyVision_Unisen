using System;
using System.Text;

namespace HslCommunication.Core.IMessage;

public class AlienMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 5;

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return true;
		}
		if (base.HeadBytes[0] == 72)
		{
			return true;
		}
		return false;
	}

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes[3] >= 48 && base.HeadBytes[3] <= 70 && base.HeadBytes[4] >= 48 && base.HeadBytes[4] <= 70)
		{
			return Convert.ToInt32(Encoding.ASCII.GetString(base.HeadBytes, 3, 2), 16);
		}
		return base.HeadBytes[3] * 256 + base.HeadBytes[4];
	}
}
