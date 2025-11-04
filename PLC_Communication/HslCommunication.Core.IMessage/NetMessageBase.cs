using System.IO;

namespace HslCommunication.Core.IMessage;

public class NetMessageBase
{
	public byte[] HeadBytes { get; set; }

	public byte[] ContentBytes { get; set; }

	public byte[] SendBytes { get; set; }

	public virtual int PependedUselesByteLength(byte[] headByte)
	{
		return 0;
	}

	public virtual int GetHeadBytesIdentity()
	{
		return 0;
	}

	public virtual bool CheckHeadBytesLegal(byte[] token)
	{
		return true;
	}

	public virtual bool CheckReceiveDataComplete(byte[] send, MemoryStream ms)
	{
		return true;
	}

	public virtual int CheckMessageMatch(byte[] send, byte[] receive)
	{
		return 1;
	}

	public override string ToString()
	{
		return GetType().Name ?? "";
	}
}
