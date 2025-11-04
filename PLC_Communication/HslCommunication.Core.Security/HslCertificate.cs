using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HslCommunication.Core.Security;

public class HslCertificate
{
	private RSACryptoServiceProvider privateRsa;

	private RSACryptoServiceProvider publicRsa;

	public string From { get; set; }

	public string To { get; set; }

	public DateTime NotBefore { get; set; }

	public DateTime NotAfter { get; set; }

	public byte[] PublicKey { get; set; }

	public DateTime CreateTime { get; set; }

	public string KeyWord { get; set; }

	public string UniqueID { get; set; }

	public int EffectiveHours { get; set; }

	public Dictionary<string, string> Descriptions { get; set; }

	public HslCertificate()
	{
		CreateTime = DateTime.Now;
	}

	public HslCertificate(RSACryptoServiceProvider pubKey, RSACryptoServiceProvider priKey)
		: this()
	{
		publicRsa = pubKey;
		privateRsa = priKey;
		PublicKey = RSAHelper.GetPublicKeyFromRSA(pubKey);
	}

	public HslCertificate(byte[] pubKey, byte[] priKey)
		: this()
	{
		if (pubKey != null)
		{
			publicRsa = RSAHelper.CreateRsaProviderFromPublicKey(pubKey);
		}
		if (priKey != null)
		{
			privateRsa = RSAHelper.CreateRsaProviderFromPrivateKey(priKey);
		}
		PublicKey = pubKey;
	}

	public void LoadFrom(byte[] hslCertificate)
	{
		int index = 4;
		PublicKey = ExtraBytes(hslCertificate, ref index);
		From = ExtraString(hslCertificate, ref index);
		To = ExtraString(hslCertificate, ref index);
		NotBefore = ExtraDateTime(hslCertificate, ref index);
		NotAfter = ExtraDateTime(hslCertificate, ref index);
		CreateTime = ExtraDateTime(hslCertificate, ref index);
		KeyWord = ExtraString(hslCertificate, ref index);
		UniqueID = ExtraString(hslCertificate, ref index);
		EffectiveHours = BitConverter.ToInt32(hslCertificate, index);
		index += 4;
		int num = ExtraShort(hslCertificate, ref index);
		Descriptions = new Dictionary<string, string>();
		for (int i = 0; i < num; i++)
		{
			string key = ExtraString(hslCertificate, ref index);
			string value = ExtraString(hslCertificate, ref index);
			Descriptions.Add(key, value);
		}
	}

	private void AddDateTime(MemoryStream ms, DateTime data)
	{
		byte[] bytes = BitConverter.GetBytes(data.Ticks);
		AddBytes(ms, bytes);
	}

	private void AddBytes(MemoryStream ms, ushort data)
	{
		byte[] bytes = BitConverter.GetBytes(data);
		ms.Write(bytes);
	}

	private void AddString(MemoryStream ms, string data)
	{
		byte[] data2 = (string.IsNullOrEmpty(data) ? null : Encoding.UTF8.GetBytes(data));
		AddBytes(ms, data2);
	}

	private void AddBytes(MemoryStream ms, byte[] data)
	{
		int num = ((data != null) ? data.Length : 0);
		ms.Write(BitConverter.GetBytes((short)num), 0, 2);
		if (data != null && num > 0)
		{
			ms.Write(data, 0, data.Length);
		}
	}

	private DateTime ExtraDateTime(byte[] buffer, ref int index)
	{
		byte[] value = ExtraBytes(buffer, ref index);
		return new DateTime(BitConverter.ToInt64(value, 0));
	}

	private ushort ExtraShort(byte[] buffer, ref int index)
	{
		ushort result = BitConverter.ToUInt16(buffer, index);
		index += 2;
		return result;
	}

	private byte[] ExtraBytes(byte[] buffer, ref int index)
	{
		int num = BitConverter.ToUInt16(buffer, index);
		index += 2;
		if (num > 0)
		{
			byte[] result = buffer.SelectMiddle(index, num);
			index += num;
			return result;
		}
		return new byte[0];
	}

	private string ExtraString(byte[] buffer, ref int index)
	{
		byte[] array = ExtraBytes(buffer, ref index);
		if (array == null || array.Length == 0)
		{
			return string.Empty;
		}
		return Encoding.UTF8.GetString(array);
	}

	public byte[] GetSaveBytes()
	{
		MemoryStream memoryStream = new MemoryStream();
		AddBytes(memoryStream, 0);
		AddBytes(memoryStream, 0);
		AddBytes(memoryStream, PublicKey);
		AddString(memoryStream, From);
		AddString(memoryStream, To);
		AddDateTime(memoryStream, NotBefore);
		AddDateTime(memoryStream, NotAfter);
		AddDateTime(memoryStream, CreateTime);
		AddString(memoryStream, KeyWord);
		AddString(memoryStream, UniqueID);
		memoryStream.Write(BitConverter.GetBytes(EffectiveHours));
		if (Descriptions == null)
		{
			AddBytes(memoryStream, 0);
		}
		else
		{
			AddBytes(memoryStream, (ushort)Descriptions.Count);
			foreach (KeyValuePair<string, string> description in Descriptions)
			{
				AddString(memoryStream, description.Key);
				AddString(memoryStream, description.Value);
			}
		}
		byte[] array = memoryStream.ToArray();
		byte[] data = privateRsa.SignData(array, 4, array.Length - 4, new SHA1CryptoServiceProvider());
		int value = array.Length - 4;
		AddBytes(memoryStream, data);
		array = memoryStream.ToArray();
		array[0] = BitConverter.GetBytes(value)[0];
		array[1] = BitConverter.GetBytes(value)[1];
		return array;
	}

	public static bool VerifyCer(byte[] publicKey, byte[] hslCertificate)
	{
		if (hslCertificate == null)
		{
			return false;
		}
		int num = BitConverter.ToUInt16(hslCertificate, 4);
		if (publicKey != null)
		{
			if (publicKey.Length != num)
			{
				return false;
			}
			for (int i = 0; i < publicKey.Length; i++)
			{
				if (publicKey[i] != hslCertificate[i + 6])
				{
					return false;
				}
			}
		}
		int num2 = BitConverter.ToUInt16(hslCertificate, 0);
		int length = BitConverter.ToUInt16(hslCertificate, num2 + 4);
		RSACryptoServiceProvider rSACryptoServiceProvider = RSAHelper.CreateRsaProviderFromPublicKey(hslCertificate.SelectMiddle(6, num));
		return rSACryptoServiceProvider.VerifyData(hslCertificate.SelectMiddle(4, num2), new SHA1CryptoServiceProvider(), hslCertificate.SelectMiddle(num2 + 6, length));
	}

	public static HslCertificate CreateFrom(byte[] hslCertificate, byte[] pubKey = null, byte[] priKey = null)
	{
		HslCertificate hslCertificate2 = new HslCertificate(pubKey, priKey);
		hslCertificate2.LoadFrom(hslCertificate);
		return hslCertificate2;
	}
}
