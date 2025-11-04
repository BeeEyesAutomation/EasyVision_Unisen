namespace HslCommunication;

public interface IDataTransfer
{
	ushort ReadCount { get; }

	void ParseSource(byte[] Content);

	byte[] ToSource();
}
