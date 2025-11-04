using System;
using System.Security.Cryptography;
using System.Text;

namespace HslCommunication.Core.Security;

public class AesCryptography : ICryptography
{
	private ICryptoTransform encryptTransform;

	private ICryptoTransform decryptTransform;

	private RijndaelManaged rijndael;

	private string key;

	public string Key => key;

	public AesCryptography(string key, CipherMode mode = CipherMode.ECB)
	{
		this.key = key;
		rijndael = new RijndaelManaged
		{
			Key = Encoding.UTF8.GetBytes(key),
			Mode = mode,
			Padding = PaddingMode.PKCS7
		};
		encryptTransform = rijndael.CreateEncryptor();
		decryptTransform = rijndael.CreateDecryptor();
	}

	public byte[] Encrypt(byte[] data)
	{
		if (data == null)
		{
			return null;
		}
		return encryptTransform.TransformFinalBlock(data, 0, data.Length);
	}

	public byte[] Decrypt(byte[] data)
	{
		if (data == null)
		{
			return null;
		}
		return decryptTransform.TransformFinalBlock(data, 0, data.Length);
	}

	public string Encrypt(string data)
	{
		byte[] data2 = (string.IsNullOrEmpty(data) ? new byte[0] : Encoding.UTF8.GetBytes(data));
		return Convert.ToBase64String(Encrypt(data2));
	}

	public string Decrypt(string data)
	{
		return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(data)));
	}
}
