using System;
using System.Text;

namespace HslCommunication.Core.IMessage;

public class MelsecQnA3EAsciiMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 18;

	public int GetContentLengthByHeadBytes()
	{
		byte[] bytes = new byte[4]
		{
			base.HeadBytes[14],
			base.HeadBytes[15],
			base.HeadBytes[16],
			base.HeadBytes[17]
		};
		return Convert.ToInt32(Encoding.ASCII.GetString(bytes), 16);
	}

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return false;
		}
		if (base.HeadBytes[0] == 68 && base.HeadBytes[1] == 48 && base.HeadBytes[2] == 48 && base.HeadBytes[3] == 48)
		{
			return true;
		}
		return false;
	}
}
