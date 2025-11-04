using System;

namespace HslCommunication.Core.IMessage;

public class SpecifiedCharacterMessage : NetMessageBase, INetMessage
{
	private int protocolHeadBytesLength = -1;

	public byte EndLength
	{
		get
		{
			return BitConverter.GetBytes(protocolHeadBytesLength)[2];
		}
		set
		{
			byte[] bytes = BitConverter.GetBytes(protocolHeadBytesLength);
			bytes[2] = value;
			protocolHeadBytesLength = BitConverter.ToInt32(bytes, 0);
		}
	}

	public int ProtocolHeadBytesLength => protocolHeadBytesLength;

	public SpecifiedCharacterMessage(byte endCode)
	{
		byte[] array = new byte[4];
		array[3] = (byte)(array[3] | 0x80);
		array[3] = (byte)(array[3] | 1);
		array[1] = endCode;
		protocolHeadBytesLength = BitConverter.ToInt32(array, 0);
	}

	public SpecifiedCharacterMessage(byte endCode1, byte endCode2)
	{
		byte[] array = new byte[4];
		array[3] = (byte)(array[3] | 0x80);
		array[3] = (byte)(array[3] | 2);
		array[1] = endCode1;
		array[0] = endCode2;
		protocolHeadBytesLength = BitConverter.ToInt32(array, 0);
	}

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckHeadBytesLegal(byte[] token)
	{
		return true;
	}
}
