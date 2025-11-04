using System.IO;

namespace HslCommunication.Core.IMessage;

public interface INetMessage
{
	int ProtocolHeadBytesLength { get; }

	byte[] HeadBytes { get; set; }

	byte[] ContentBytes { get; set; }

	byte[] SendBytes { get; set; }

	int GetContentLengthByHeadBytes();

	int PependedUselesByteLength(byte[] headByte);

	bool CheckHeadBytesLegal(byte[] token);

	int GetHeadBytesIdentity();

	bool CheckReceiveDataComplete(byte[] send, MemoryStream ms);

	int CheckMessageMatch(byte[] send, byte[] receive);
}
