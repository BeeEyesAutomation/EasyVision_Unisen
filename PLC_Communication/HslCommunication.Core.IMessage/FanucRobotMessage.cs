using System;

namespace HslCommunication.Core.IMessage;

public class FanucRobotMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 56;

	public int GetContentLengthByHeadBytes()
	{
		return BitConverter.ToUInt16(base.HeadBytes, 4);
	}
}
