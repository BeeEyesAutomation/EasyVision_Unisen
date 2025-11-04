using System;
using System.Text;

namespace HslCommunication.Core.IMessage;

public class OpenProtocolMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 4;

	public int GetContentLengthByHeadBytes()
	{
		try
		{
			byte[] headBytes = base.HeadBytes;
			if (headBytes != null && headBytes.Length >= 4)
			{
				int num = Convert.ToInt32(Encoding.ASCII.GetString(base.HeadBytes, 0, 4)) - 4 + 1;
				return (num >= 0) ? num : 0;
			}
			return 0;
		}
		catch
		{
			return 17;
		}
	}
}
