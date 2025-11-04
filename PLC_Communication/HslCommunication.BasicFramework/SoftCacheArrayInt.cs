using System;

namespace HslCommunication.BasicFramework;

public sealed class SoftCacheArrayInt : SoftCacheArrayBase
{
	private int[] DataArray = null;

	public SoftCacheArrayInt(int capacity, int defaultValue)
	{
		if (capacity < 10)
		{
			capacity = 10;
		}
		base.ArrayLength = capacity;
		DataArray = new int[capacity];
		DataBytes = new byte[capacity * 4];
		if (defaultValue != 0)
		{
			for (int i = 0; i < capacity; i++)
			{
				DataArray[i] = defaultValue;
			}
		}
	}

	public override void LoadFromBytes(byte[] dataSave)
	{
		int num = (base.ArrayLength = dataSave.Length / 4);
		int num3 = num;
		DataArray = new int[num3];
		DataBytes = new byte[num3 * 4];
		for (int i = 0; i < num3; i++)
		{
			DataArray[i] = BitConverter.ToInt32(dataSave, i * 4);
		}
	}

	public void AddValue(int value)
	{
		HybirdLock.Enter();
		for (int i = 0; i < base.ArrayLength - 1; i++)
		{
			DataArray[i] = DataArray[i + 1];
		}
		DataArray[base.ArrayLength - 1] = value;
		for (int j = 0; j < base.ArrayLength; j++)
		{
			BitConverter.GetBytes(DataArray[j]).CopyTo(DataBytes, 4 * j);
		}
		HybirdLock.Leave();
	}

	public int[] GetIntArray()
	{
		int[] array = null;
		HybirdLock.Enter();
		array = new int[base.ArrayLength];
		for (int i = 0; i < base.ArrayLength; i++)
		{
			array[i] = DataArray[i];
		}
		HybirdLock.Leave();
		return array;
	}
}
