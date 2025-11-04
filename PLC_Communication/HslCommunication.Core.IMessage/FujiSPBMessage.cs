using System;
using System.IO;
using System.Text;
using HslCommunication.ModBus;

namespace HslCommunication.Core.IMessage;

public class FujiSPBMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => 5;

	public int GetContentLengthByHeadBytes()
	{
		if (base.HeadBytes == null)
		{
			return 0;
		}
		return Convert.ToInt32(Encoding.ASCII.GetString(base.HeadBytes, 3, 2), 16) * 2 + 2;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return ModbusInfo.CheckAsciiReceiveDataComplete(ms.ToArray());
	}
}
