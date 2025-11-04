using System;
using HslCommunication.Core.IMessage;

namespace HslCommunication.Secs.Message;

public class SecsHsmsMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 4;

	public int GetContentLengthByHeadBytes()
	{
		int num = BitConverter.ToInt32(new byte[4]
		{
			base.HeadBytes[3],
			base.HeadBytes[2],
			base.HeadBytes[1],
			base.HeadBytes[0]
		}, 0);
		if (num < 0)
		{
			return 0;
		}
		return num;
	}

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		return true;
	}
}
