namespace HslCommunication.Core.IMessage;

public class DcsNanJingAutoMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 6;

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes?.Length >= ProtocolHeadBytesLength)
		{
			return base.HeadBytes[4] * 256 + base.HeadBytes[5];
		}
		return 0;
	}

	public override int GetHeadBytesIdentity()
	{
		return base.HeadBytes[0] * 256 + base.HeadBytes[1];
	}
}
