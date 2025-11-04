namespace HslCommunication.Core.IMessage;

public class GeSRTPMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 56;

	public int GetContentLengthByHeadBytes()
	{
		return base.HeadBytes[4] + base.HeadBytes[5] * 256;
	}
}
