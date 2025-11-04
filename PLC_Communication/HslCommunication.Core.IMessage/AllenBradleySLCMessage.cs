namespace HslCommunication.Core.IMessage;

public class AllenBradleySLCMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 28;

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes == null)
		{
			return 0;
		}
		return base.HeadBytes[2] * 256 + base.HeadBytes[3];
	}
}
