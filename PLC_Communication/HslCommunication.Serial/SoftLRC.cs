using System;
using System.Text;
using HslCommunication.BasicFramework;

namespace HslCommunication.Serial;

public class SoftLRC
{
	public static byte[] LRC(byte[] value)
	{
		if (value == null)
		{
			return null;
		}
		int num = 0;
		for (int i = 0; i < value.Length; i++)
		{
			num += value[i];
		}
		num %= 256;
		num = 256 - num;
		byte[] array = new byte[1] { (byte)num };
		return SoftBasic.SpliceArray<byte>(value, array);
	}

	public static byte LRC(byte[] value, int left, int right)
	{
		int num = 0;
		for (int i = left; i < value.Length - right; i++)
		{
			num += value[i];
		}
		num %= 256;
		num = 256 - num;
		return (byte)num;
	}

	public static bool CheckLRC(byte[] value)
	{
		if (value == null)
		{
			return false;
		}
		int num = value.Length;
		byte[] array = new byte[num - 1];
		Array.Copy(value, 0, array, 0, array.Length);
		byte[] array2 = LRC(array);
		if (array2[num - 1] == value[num - 1])
		{
			return true;
		}
		return false;
	}

	public static int CalculateAcc(byte[] buffer)
	{
		return CalculateAcc(buffer, 0, 0);
	}

	public static int CalculateAcc(byte[] buffer, int headCount, int lastCount)
	{
		int num = 0;
		for (int i = headCount; i < buffer.Length - lastCount; i++)
		{
			num += buffer[i];
		}
		return num;
	}

	public static void CalculateAccAndFill(byte[] buffer, int headCount, int lastCount)
	{
		byte b = (byte)CalculateAcc(buffer, headCount, lastCount);
		Encoding.ASCII.GetBytes(b.ToString("X2")).CopyTo(buffer, buffer.Length - lastCount);
	}

	public static bool CalculateAccAndCheck(byte[] buffer, int headCount, int lastCount)
	{
		return ((byte)CalculateAcc(buffer, headCount, lastCount)).ToString("X2") == Encoding.ASCII.GetString(buffer, buffer.Length - lastCount, 2);
	}

	public static byte[] CalculateFcs(byte[] source, int left, int right)
	{
		int num = source[left];
		for (int i = left + 1; i < source.Length - right; i++)
		{
			num ^= source[i];
		}
		return new byte[2]
		{
			SoftBasic.BuildAsciiBytesFrom((byte)num)[0],
			SoftBasic.BuildAsciiBytesFrom((byte)num)[1]
		};
	}
}
