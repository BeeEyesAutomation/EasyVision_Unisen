using HslCommunication.Core.IMessage;

namespace HslCommunication.CNC.Fanuc;

public class CNCFanucSeriesMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 10;

	public int GetContentLengthByHeadBytes()
	{
		return base.HeadBytes[8] * 256 + base.HeadBytes[9];
	}

	public override string ToString()
	{
		return "CNCFanucSeriesMessage";
	}
}
