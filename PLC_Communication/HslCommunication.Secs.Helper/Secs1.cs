using System;
using System.Collections.Generic;
using HslCommunication.BasicFramework;
using HslCommunication.Serial;

namespace HslCommunication.Secs.Helper;

public class Secs1
{
	public static List<byte[]> BuildSecsOneMessage(ushort deviceID, byte streamNo, byte functionNo, ushort blockNo, uint messageID, byte[] data, bool wBit)
	{
		List<byte[]> list = new List<byte[]>();
		List<byte[]> list2 = ((data.Length <= 244) ? SoftBasic.ArraySplitByLength(data, 244) : SoftBasic.ArraySplitByLength(data, 224));
		for (int i = 0; i < list2.Count; i++)
		{
			byte[] array = new byte[13 + list2[i].Length];
			array[0] = (byte)(10 + list2[i].Length);
			array[1] = BitConverter.GetBytes(deviceID)[1];
			array[2] = BitConverter.GetBytes(deviceID)[0];
			array[3] = (wBit ? ((byte)(streamNo | 0x80)) : streamNo);
			array[4] = functionNo;
			array[5] = ((i == list2.Count - 1) ? ((byte)(BitConverter.GetBytes(blockNo)[1] | 0x80)) : BitConverter.GetBytes(blockNo)[1]);
			array[6] = BitConverter.GetBytes(blockNo)[0];
			array[7] = BitConverter.GetBytes(messageID)[3];
			array[8] = BitConverter.GetBytes(messageID)[2];
			array[9] = BitConverter.GetBytes(messageID)[1];
			array[10] = BitConverter.GetBytes(messageID)[0];
			list2[i].CopyTo(array, 11);
			int value = SoftLRC.CalculateAcc(array, 1, 2);
			array[array.Length - 2] = BitConverter.GetBytes(value)[1];
			array[array.Length - 1] = BitConverter.GetBytes(value)[0];
			list.Add(array);
		}
		return list;
	}

	public static byte[] BuildHSMSMessage(ushort deviceID, byte streamNo, byte functionNo, ushort blockNo, uint messageID, byte[] data, bool wBit)
	{
		if (data == null)
		{
			data = new byte[0];
		}
		byte[] array = new byte[14 + data.Length];
		array[0] = BitConverter.GetBytes(array.Length - 4)[3];
		array[1] = BitConverter.GetBytes(array.Length - 4)[2];
		array[2] = BitConverter.GetBytes(array.Length - 4)[1];
		array[3] = BitConverter.GetBytes(array.Length - 4)[0];
		array[4] = BitConverter.GetBytes(deviceID)[1];
		array[5] = BitConverter.GetBytes(deviceID)[0];
		array[6] = (wBit ? ((byte)(streamNo | 0x80)) : streamNo);
		array[7] = functionNo;
		array[8] = BitConverter.GetBytes(blockNo)[1];
		array[9] = BitConverter.GetBytes(blockNo)[0];
		array[10] = BitConverter.GetBytes(messageID)[3];
		array[11] = BitConverter.GetBytes(messageID)[2];
		array[12] = BitConverter.GetBytes(messageID)[1];
		array[13] = BitConverter.GetBytes(messageID)[0];
		data.CopyTo(array, 14);
		return array;
	}
}
