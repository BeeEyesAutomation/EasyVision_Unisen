namespace HslCommunication.Core.IMessage;

public class SiemensPPIServerMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 6;

	public int GetContentLengthByHeadBytes()
	{
		return base.HeadBytes[1];
	}
}
