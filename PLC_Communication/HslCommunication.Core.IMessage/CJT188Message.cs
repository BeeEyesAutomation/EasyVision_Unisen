using System.IO;
using HslCommunication.Instrument.DLT.Helper;

namespace HslCommunication.Core.IMessage;

public class CJT188Message : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 11;

	public bool StationMatch { get; set; } = false;

	public CJT188Message(bool stationMatch)
	{
		StationMatch = stationMatch;
	}

	public int GetContentLengthByHeadBytes()
	{
		return base.HeadBytes[10] + 2;
	}

	public override int PependedUselesByteLength(byte[] headByte)
	{
		return DLT645Helper.FindHeadCode68H(headByte);
	}

	public override int CheckMessageMatch(byte[] send, byte[] receive)
	{
		if (!StationMatch)
		{
			return 1;
		}
		if (send.Length < 9 || receive.Length < 9)
		{
			return 1;
		}
		string text = send.SelectMiddle(2, 7).ToHexString();
		string text2 = receive.SelectMiddle(2, 7).ToHexString();
		if (text == "AAAAAAAAAAAAAA" || text2 == "AAAAAAAAAAAAAA" || text == text2)
		{
			return 1;
		}
		return -1;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		byte[] array = ms.ToArray();
		if (array.Length < 11)
		{
			return false;
		}
		int num = DLT645Helper.FindHeadCode68H(array);
		if (num < 0)
		{
			return false;
		}
		if (array[num + 10] + 13 + num == array.Length && array[array.Length - 1] == 22)
		{
			return true;
		}
		return false;
	}
}
