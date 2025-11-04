namespace HslCommunication.Core.IMessage;

public class YokogawaLinkBinaryMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 4;

	public int GetContentLengthByHeadBytes()
	{
		return base.HeadBytes[2] * 256 + base.HeadBytes[3];
	}
}
