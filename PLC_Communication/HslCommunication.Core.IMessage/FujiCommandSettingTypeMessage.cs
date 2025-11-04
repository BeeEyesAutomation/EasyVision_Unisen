namespace HslCommunication.Core.IMessage;

public class FujiCommandSettingTypeMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 5;

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes == null)
		{
			return 0;
		}
		return base.HeadBytes[4];
	}
}
