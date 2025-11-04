using HslCommunication.Core.IMessage;

namespace HslCommunication.Profinet.Geniitek;

public class VibrationSensorLongMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 12;

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		if (base.HeadBytes == null)
		{
			return false;
		}
		if (base.HeadBytes[0] == 170 && base.HeadBytes[1] == 85 && base.HeadBytes[2] == 127)
		{
			return true;
		}
		return false;
	}

	public int GetContentLengthByHeadBytes()
	{
		return base.HeadBytes[10] * 256 + base.HeadBytes[11] + 4;
	}
}
