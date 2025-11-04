using System.IO;
using HslCommunication.Instrument.DLT.Helper;

namespace HslCommunication.Core.IMessage;

public class DLT645Message : NetMessageBase, INetMessage
{
	private bool checkDataId = true;

	public int ProtocolHeadBytesLength => 10;

	public DLT645Message()
	{
	}

	public DLT645Message(bool checkDataId)
	{
		this.checkDataId = checkDataId;
	}

	public int GetContentLengthByHeadBytes()
	{
		return base.HeadBytes[9] + 2;
	}

	public override int PependedUselesByteLength(byte[] headByte)
	{
		int num = DLT645Helper.FindHeadCode68H(headByte);
		if (num < 0)
		{
			return 10;
		}
		return num;
	}

	public override int CheckMessageMatch(byte[] send, byte[] receive)
	{
		if (!checkDataId)
		{
			return base.CheckMessageMatch(send, receive);
		}
		if (send == null || receive == null || send.Length < 16 || receive.Length < 16)
		{
			return base.CheckMessageMatch(send, receive);
		}
		int num = DLT645Helper.FindHeadCode68H(send);
		int num2 = DLT645Helper.FindHeadCode68H(receive);
		if (num < 0 || num2 < 0 || num > 4 || num2 > 4)
		{
			return base.CheckMessageMatch(send, receive);
		}
		if (send[num + 8] == 17 && receive[num2 + 8] == 145)
		{
			for (int i = 0; i < 4; i++)
			{
				if (num + 10 + i >= send.Length)
				{
					return 1;
				}
				if (num2 + 10 + i >= receive.Length)
				{
					return 1;
				}
				if (send[num + 10 + i] != receive[num2 + 10 + i])
				{
					return -1;
				}
			}
			return 1;
		}
		if (send[num + 8] == 1 && receive[num2 + 8] == 129)
		{
			for (int j = 0; j < 2; j++)
			{
				if (num + 10 + j >= send.Length)
				{
					return 1;
				}
				if (num2 + 10 + j >= receive.Length)
				{
					return 1;
				}
				if (send[num + 10 + j] != receive[num2 + 10 + j])
				{
					return -1;
				}
			}
			return 1;
		}
		return base.CheckMessageMatch(send, receive);
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return DLT645Helper.CheckReceiveDataComplete(ms);
	}
}
