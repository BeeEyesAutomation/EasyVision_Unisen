namespace HslCommunication.Core.IMessage;

public class KukaVarProxyMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 4;

	public int GetContentLengthByHeadBytes()
	{
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 4)
		{
			return base.HeadBytes[2] * 256 + base.HeadBytes[3];
		}
		return 0;
	}

	public override int GetHeadBytesIdentity()
	{
		byte[] headBytes = base.HeadBytes;
		if (headBytes != null && headBytes.Length >= 4)
		{
			return base.HeadBytes[0] * 256 + base.HeadBytes[1];
		}
		return 0;
	}
}
