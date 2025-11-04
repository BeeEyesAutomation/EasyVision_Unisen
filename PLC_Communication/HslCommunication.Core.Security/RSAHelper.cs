using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core.Security;

public class RSAHelper
{
	private const string privateKeyHead = "-----BEGIN RSA PRIVATE KEY-----";

	private const string privateKeyEnd = "-----END RSA PRIVATE KEY-----";

	private const string publicKeyHead = "-----BEGIN PUBLIC KEY-----";

	private const string publicKeyEnd = "-----END PUBLIC KEY-----";

	private static readonly byte[] SeqOID = new byte[15]
	{
		48, 13, 6, 9, 42, 134, 72, 134, 247, 13,
		1, 1, 1, 5, 0
	};

	public static RSACryptoServiceProvider CreateRsaProviderFromPrivateKey(string privateKeyString)
	{
		privateKeyString = privateKeyString.Trim();
		if (privateKeyString.StartsWith("-----BEGIN RSA PRIVATE KEY-----"))
		{
			privateKeyString = privateKeyString.Replace("-----BEGIN RSA PRIVATE KEY-----", string.Empty);
		}
		if (privateKeyString.EndsWith("-----END RSA PRIVATE KEY-----"))
		{
			privateKeyString = privateKeyString.Replace("-----END RSA PRIVATE KEY-----", string.Empty);
		}
		byte[] privateKey = Convert.FromBase64String(privateKeyString);
		return CreateRsaProviderFromPrivateKey(privateKey);
	}

	public static RSACryptoServiceProvider CreateRsaProviderFromPrivateKey(byte[] privateKey)
	{
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		RSAParameters parameters = default(RSAParameters);
		using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(privateKey)))
		{
			byte b = 0;
			ushort num = 0;
			switch (binaryReader.ReadUInt16())
			{
			case 33072:
				binaryReader.ReadByte();
				break;
			case 33328:
				binaryReader.ReadInt16();
				break;
			default:
				throw new Exception("Unexpected value read binr.ReadUInt16()");
			}
			num = binaryReader.ReadUInt16();
			if (num != 258)
			{
				throw new Exception("Unexpected version");
			}
			if (binaryReader.ReadByte() != 0)
			{
				throw new Exception("Unexpected value read binr.ReadByte()");
			}
			parameters.Modulus = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
			parameters.Exponent = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
			parameters.D = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
			parameters.P = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
			parameters.Q = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
			parameters.DP = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
			parameters.DQ = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
			parameters.InverseQ = binaryReader.ReadBytes(GetIntegerSize(binaryReader));
		}
		rSACryptoServiceProvider.ImportParameters(parameters);
		return rSACryptoServiceProvider;
	}

	private static int GetIntegerSize(BinaryReader binr)
	{
		byte b = 0;
		byte b2 = 0;
		byte b3 = 0;
		int num = 0;
		b = binr.ReadByte();
		if (b != 2)
		{
			return 0;
		}
		b = binr.ReadByte();
		switch (b)
		{
		case 129:
			num = binr.ReadByte();
			break;
		case 130:
		{
			b3 = binr.ReadByte();
			b2 = binr.ReadByte();
			byte[] value = new byte[4] { b2, b3, 0, 0 };
			num = BitConverter.ToInt32(value, 0);
			break;
		}
		default:
			num = b;
			break;
		}
		while (binr.ReadByte() == 0)
		{
			num--;
		}
		binr.BaseStream.Seek(-1L, SeekOrigin.Current);
		return num;
	}

	private static byte[] PackKeyHead(byte[] content)
	{
		if (content.Length < 256)
		{
			byte[] array = SoftBasic.SpliceArray<byte>("30 81 00".ToHexBytes(), content);
			array[2] = BitConverter.GetBytes(array.Length - 3)[0];
			return array;
		}
		byte[] array2 = SoftBasic.SpliceArray<byte>("30 82 00 00".ToHexBytes(), content);
		array2[2] = BitConverter.GetBytes(array2.Length - 4)[1];
		array2[3] = BitConverter.GetBytes(array2.Length - 4)[0];
		return array2;
	}

	public static byte[] GetPrivateKeyFromRSA(RSACryptoServiceProvider rsa)
	{
		RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters: true);
		byte[] modulus = rSAParameters.Modulus;
		byte[] exponent = rSAParameters.Exponent;
		byte[] d = rSAParameters.D;
		byte[] p = rSAParameters.P;
		byte[] q = rSAParameters.Q;
		byte[] dP = rSAParameters.DP;
		byte[] dQ = rSAParameters.DQ;
		byte[] inverseQ = rSAParameters.InverseQ;
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(new byte[3] { 2, 1, 0 }, 0, 3);
		WriteByteStream(memoryStream, modulus);
		WriteByteStream(memoryStream, exponent);
		WriteByteStream(memoryStream, d);
		WriteByteStream(memoryStream, p);
		WriteByteStream(memoryStream, q);
		WriteByteStream(memoryStream, dP);
		WriteByteStream(memoryStream, dQ);
		WriteByteStream(memoryStream, inverseQ);
		return PackKeyHead(memoryStream.ToArray());
	}

	private static void WriteByteStream(MemoryStream ms, byte[] data)
	{
		bool flag = data[0] > 127;
		int num = (flag ? (data.Length + 1) : data.Length);
		ms.WriteByte(2);
		if (num < 128)
		{
			ms.WriteByte((byte)num);
		}
		else if (num < 256)
		{
			ms.WriteByte(129);
			ms.WriteByte((byte)num);
		}
		else
		{
			ms.WriteByte(130);
			ms.WriteByte(BitConverter.GetBytes(num)[1]);
			ms.WriteByte(BitConverter.GetBytes(num)[0]);
		}
		if (flag)
		{
			ms.WriteByte(0);
		}
		ms.Write(data, 0, data.Length);
	}

	public static byte[] GetPublicKeyFromRSA(RSACryptoServiceProvider rsa)
	{
		RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters: false);
		byte[] modulus = rSAParameters.Modulus;
		byte[] exponent = rSAParameters.Exponent;
		MemoryStream memoryStream = new MemoryStream();
		WriteByteStream(memoryStream, modulus);
		WriteByteStream(memoryStream, exponent);
		byte[] array = PackKeyHead(SoftBasic.SpliceArray<byte>(new byte[1], PackKeyHead(memoryStream.ToArray())));
		array[0] = 3;
		return PackKeyHead(SoftBasic.SpliceArray<byte>(SeqOID, array));
	}

	public static RSACryptoServiceProvider CreateRsaProviderFromPublicKey(string publicKeyString)
	{
		publicKeyString = publicKeyString.Trim();
		if (publicKeyString.StartsWith("-----BEGIN PUBLIC KEY-----"))
		{
			publicKeyString = publicKeyString.Replace("-----BEGIN PUBLIC KEY-----", string.Empty);
		}
		if (publicKeyString.EndsWith("-----END PUBLIC KEY-----"))
		{
			publicKeyString = publicKeyString.Replace("-----END PUBLIC KEY-----", string.Empty);
		}
		return CreateRsaProviderFromPublicKey(Convert.FromBase64String(publicKeyString));
	}

	public static byte[] EncryptLargeDataByRSA(RSACryptoServiceProvider provider, byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream();
		List<byte[]> list = SoftBasic.ArraySplitByLength(data, 110);
		for (int i = 0; i < list.Count; i++)
		{
			byte[] array = provider.Encrypt(list[i], fOAEP: false);
			memoryStream.Write(array, 0, array.Length);
		}
		return memoryStream.ToArray();
	}

	public static byte[] DecryptLargeDataByRSA(RSACryptoServiceProvider provider, byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream();
		List<byte[]> list = SoftBasic.ArraySplitByLength(data, 128);
		for (int i = 0; i < list.Count; i++)
		{
			byte[] array = provider.Decrypt(list[i], fOAEP: false);
			memoryStream.Write(array, 0, array.Length);
		}
		return memoryStream.ToArray();
	}

	public static RSACryptoServiceProvider CreateRsaProviderFromPublicKey(byte[] publicKey)
	{
		byte[] array = new byte[15];
		int num = publicKey.Length;
		using MemoryStream input = new MemoryStream(publicKey);
		using BinaryReader binaryReader = new BinaryReader(input);
		byte b = 0;
		ushort num2 = 0;
		switch (binaryReader.ReadUInt16())
		{
		case 33072:
			binaryReader.ReadByte();
			break;
		case 33328:
			binaryReader.ReadInt16();
			break;
		default:
			return null;
		}
		array = binaryReader.ReadBytes(15);
		if (!CompareBytearrays(array, SeqOID))
		{
			return null;
		}
		switch (binaryReader.ReadUInt16())
		{
		case 33027:
			binaryReader.ReadByte();
			break;
		case 33283:
			binaryReader.ReadInt16();
			break;
		default:
			return null;
		}
		if (binaryReader.ReadByte() != 0)
		{
			return null;
		}
		switch (binaryReader.ReadUInt16())
		{
		case 33072:
			binaryReader.ReadByte();
			break;
		case 33328:
			binaryReader.ReadInt16();
			break;
		default:
			return null;
		}
		num2 = binaryReader.ReadUInt16();
		byte b2 = 0;
		byte b3 = 0;
		switch (num2)
		{
		case 33026:
			b2 = binaryReader.ReadByte();
			break;
		case 33282:
			b3 = binaryReader.ReadByte();
			b2 = binaryReader.ReadByte();
			break;
		default:
			return null;
		}
		byte[] value = new byte[4] { b2, b3, 0, 0 };
		int num3 = BitConverter.ToInt32(value, 0);
		if (binaryReader.PeekChar() == 0)
		{
			binaryReader.ReadByte();
			num3--;
		}
		byte[] modulus = binaryReader.ReadBytes(num3);
		if (binaryReader.ReadByte() != 2)
		{
			return null;
		}
		int count = binaryReader.ReadByte();
		byte[] exponent = binaryReader.ReadBytes(count);
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.ImportParameters(new RSAParameters
		{
			Modulus = modulus,
			Exponent = exponent
		});
		return rSACryptoServiceProvider;
	}

	private static bool CompareBytearrays(byte[] a, byte[] b)
	{
		if (a.Length != b.Length)
		{
			return false;
		}
		int num = 0;
		foreach (byte b2 in a)
		{
			if (b2 != b[num])
			{
				return false;
			}
			num++;
		}
		return true;
	}
}
