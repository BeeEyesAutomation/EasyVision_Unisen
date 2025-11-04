namespace HslCommunication.Core.Security;

public interface ICryptography
{
	string Key { get; }

	byte[] Encrypt(byte[] data);

	byte[] Decrypt(byte[] data);

	string Encrypt(string data);

	string Decrypt(string data);
}
