using System;
using System.Security.Cryptography;
using System.Text;

namespace HslCommunication.Core.Security;

public class DesCryptography : ICryptography
{
	private ICryptoTransform encryptTransform;

	private ICryptoTransform decryptTransform;

	private DESCryptoServiceProvider des;

	private string key;

	public string Key => key;

	public DesCryptography(string key)
	{
		this.key = key;
		des = new DESCryptoServiceProvider();
		des.Key = Encoding.ASCII.GetBytes(key);
		des.IV = Encoding.ASCII.GetBytes(key);
		encryptTransform = des.CreateEncryptor();
		decryptTransform = des.CreateDecryptor();
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
