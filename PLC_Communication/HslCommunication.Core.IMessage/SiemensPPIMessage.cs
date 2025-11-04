using System.IO;
using HslCommunication.Profinet.Siemens.Helper;

namespace HslCommunication.Core.IMessage;

public class SiemensPPIMessage : NetMessageBase, INetMessage
{
	public int ProtocolHeadBytesLength => -1;

	public int GetContentLengthByHeadBytes()
	{
		return 0;
	}

	public override bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return SiemensPPIHelper.CheckReceiveDataComplete(ms);
	}
}
