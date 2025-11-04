using System;
using System.Text;
using HslCommunication.BasicFramework;

namespace HslCommunication.Robot.Hyundai;

public class HyundaiData
{
	public char Command { get; set; }

	public string CharDummy { get; set; }

	public int State { get; set; }

	public int Count { get; set; }

	public int IntDummy { get; set; }

	public double[] Data { get; set; }

	public HyundaiData()
	{
		Data = new double[6];
	}

	public HyundaiData(byte[] buffer)
	{
		LoadBy(buffer);
	}

	public void LoadBy(byte[] buffer, int index = 0)
	{
		Command = (char)buffer[index];
		CharDummy = Encoding.ASCII.GetString(buffer, index + 1, 3);
		State = BitConverter.ToInt32(buffer, index + 4);
		Count = BitConverter.ToInt32(buffer, index + 8);
		IntDummy = BitConverter.ToInt32(buffer, index + 12);
		Data = new double[6];
		for (int i = 0; i < Data.Length; i++)
		{
			if (i < 3)
			{
				Data[i] = BitConverter.ToDouble(buffer, index + 16 + 8 * i) * 1000.0;
			}
			else
			{
				Data[i] = BitConverter.ToDouble(buffer, index + 16 + 8 * i) * 180.0 / Math.PI;
			}
		}
	}

	public byte[] ToBytes()
	{
		byte[] array = new byte[64];
		array[0] = (byte)Command;
		if (!string.IsNullOrEmpty(CharDummy))
		{
			Encoding.ASCII.GetBytes(CharDummy).CopyTo(array, 1);
		}
		BitConverter.GetBytes(State).CopyTo(array, 4);
		BitConverter.GetBytes(Count).CopyTo(array, 8);
		BitConverter.GetBytes(IntDummy).CopyTo(array, 12);
		for (int i = 0; i < Data.Length; i++)
		{
			if (i < 3)
			{
				BitConverter.GetBytes(Data[i] / 1000.0).CopyTo(array, 16 + 8 * i);
			}
			else
			{
				BitConverter.GetBytes(Data[i] * Math.PI / 180.0).CopyTo(array, 16 + 8 * i);
			}
		}
		return array;
	}

	public override string ToString()
	{
		return $"HyundaiData:Cmd[{Command},{CharDummy},{State},{Count},{IntDummy}] Data:{SoftBasic.ArrayFormat(Data)}";
	}
}
