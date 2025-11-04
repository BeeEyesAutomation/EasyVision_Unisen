namespace HslCommunication;

internal class HslSecurity
{
	internal static byte[] ByteEncrypt(byte[] enBytes)
	{
		if (enBytes == null)
		{
			return null;
		}
		byte[] array = new byte[enBytes.Length];
		for (int i = 0; i < enBytes.Length; i++)
		{
			array[i] = (byte)(enBytes[i] ^ 0xB5);
		}
		return array;
	}

	internal static void ByteEncrypt(byte[] enBytes, int offset, int count)
	{
		for (int i = offset; i < offset + count && i < enBytes.Length; i++)
		{
			enBytes[i] ^= 0xB5;
		}
	}

	internal static byte[] ByteDecrypt(byte[] deBytes)
	{
		return ByteEncrypt(deBytes);
	}
}
