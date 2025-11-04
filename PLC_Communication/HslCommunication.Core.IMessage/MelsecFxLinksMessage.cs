using System.IO;

namespace HslCommunication.Core.IMessage;

public class MelsecFxLinksMessage : NetMessageBase, INetMessage
{
	private int format = 1;

	private bool sumCheck = true;

	public int ProtocolHeadBytesLength => -1;

	public MelsecFxLinksMessage(int format, bool sumCheck)
	{
		this.format = format;
		this.sumCheck = sumCheck;
	}

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		byte[] array = ms.ToArray();
		if (array.Length < 5)
		{
			return false;
		}
		if (format == 1)
		{
			if (array[0] == 21)
			{
				return array.Length == 7;
			}
			if (array[0] == 6)
			{
				return array.Length == 5;
			}
			if (array[0] == 2)
			{
				if (sumCheck)
				{
					return array[array.Length - 3] == 3;
				}
				return array[array.Length - 1] == 3;
			}
			return false;
		}
		if (format == 4)
		{
			return array[array.Length - 1] == 10 && array[array.Length - 2] == 13;
		}
		return false;
	}
}
