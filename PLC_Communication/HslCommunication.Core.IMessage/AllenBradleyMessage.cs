using System;

namespace HslCommunication.Core.IMessage;

public class AllenBradleyMessage : NetMessageBase, INetMessage
{
	private bool contextCheck = false;

	public int ProtocolHeadBytesLength => 24;

	public AllenBradleyMessage()
	{
	}

	public AllenBradleyMessage(bool contextCheck)
	{
		this.contextCheck = contextCheck;
	}

	public int GetContentLengthByHeadBytes()
	{
		return BitConverter.ToUInt16(base.HeadBytes, 2);
	}

	public override int CheckMessageMatch(byte[] send, byte[] receive)
	{
		if (receive == null || receive.Length < 24 || send == null || send.Length < 24)
		{
			return 1;
		}
		if (contextCheck)
		{
			for (int i = 12; i < 20; i++)
			{
				if (receive[i] != send[i])
				{
					return -1;
				}
			}
		}
		return base.CheckMessageMatch(send, receive);
	}
}
