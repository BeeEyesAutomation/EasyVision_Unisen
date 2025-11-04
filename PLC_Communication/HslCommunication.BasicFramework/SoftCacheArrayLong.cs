using System;

namespace HslCommunication.BasicFramework;

public sealed class SoftCacheArrayLong : SoftCacheArrayBase
{
	private long[] DataArray = null;

	public SoftCacheArrayLong(int capacity, int defaultValue)
	{
		if (capacity < 10)
		{
			capacity = 10;
		}
		base.ArrayLength = capacity;
		DataArray = new long[capacity];
		DataBytes = new byte[capacity * 8];
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
		int num = (base.ArrayLength = dataSave.Length / 8);
		int num3 = num;
		DataArray = new long[num3];
		DataBytes = new byte[num3 * 8];
		for (int i = 0; i < num3; i++)
		{
			DataArray[i] = BitConverter.ToInt64(dataSave, i * 8);
		}
	}

	public void AddValue(long value)
	{
		HybirdLock.Enter();
		for (int i = 0; i < base.ArrayLength - 1; i++)
		{
			DataArray[i] = DataArray[i + 1];
		}
		DataArray[base.ArrayLength - 1] = value;
		for (int j = 0; j < base.ArrayLength; j++)
		{
			BitConverter.GetBytes(DataArray[j]).CopyTo(DataBytes, 8 * j);
		}
		HybirdLock.Leave();
	}
}
