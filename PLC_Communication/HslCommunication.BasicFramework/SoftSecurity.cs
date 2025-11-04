using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HslCommunication.BasicFramework;

public static class SoftSecurity
{
	internal static string MD5Encrypt(string pToEncrypt)
	{
		return MD5Encrypt(pToEncrypt, "zxcvBNMM");
	}

	public static string MD5Encrypt(string pToEncrypt, string Password)
	{
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		byte[] bytes = Encoding.Default.GetBytes(pToEncrypt);
		dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(Password);
		dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(Password);
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(bytes, 0, bytes.Length);
		cryptoStream.FlushFinalBlock();
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = memoryStream.ToArray();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.AppendFormat("{0:X2}", b);
		}
		stringBuilder.ToString();
		return stringBuilder.ToString();
	}

	internal static string MD5Decrypt(string pToDecrypt)
	{
		return MD5Decrypt(pToDecrypt, "zxcvBNMM");
	}

	public static string MD5Decrypt(string pToDecrypt, string password)
	{
		if (pToDecrypt == "")
		{
			return pToDecrypt;
		}
		DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
		byte[] array = new byte[pToDecrypt.Length / 2];
		for (int i = 0; i < pToDecrypt.Length / 2; i++)
		{
			int num = Convert.ToInt32(pToDecrypt.Substring(i * 2, 2), 16);
			array[i] = (byte)num;
		}
		dESCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(password);
		dESCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(password);
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(array, 0, array.Length);
		cryptoStream.FlushFinalBlock();
		cryptoStream.Dispose();
		return Encoding.Default.GetString(memoryStream.ToArray());
	}
}
