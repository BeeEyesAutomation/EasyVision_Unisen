using System;
using HslCommunication.Instrument.DLT.Helper;

namespace HslCommunication.Core.IMessage;

public class DLT698Message : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 8;

	public int GetContentLengthByHeadBytes()
	{
		return BitConverter.ToUInt16(base.HeadBytes, 1) + 2 - 8;
	}

	public override int PependedUselesByteLength(byte[] headByte)
	{
		return DLT645Helper.FindHeadCode68H(headByte);
	}
}
