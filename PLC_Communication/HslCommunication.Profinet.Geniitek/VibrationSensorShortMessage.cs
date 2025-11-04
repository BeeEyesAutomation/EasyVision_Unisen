using HslCommunication.Core.IMessage;

namespace HslCommunication.Profinet.Geniitek;

public class VibrationSensorShortMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 9;

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return false;
		}
		if (base.HeadBytes[0] == 170)
		{
			return true;
		}
		return false;
	}

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}
}
