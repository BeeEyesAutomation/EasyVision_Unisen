namespace HslCommunication.Core.IMessage;

public class IEC104Message : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 2;

	public int GetContentLengthByHeadBytes()
	{
		return base.HeadBytes[1];
	}
}
