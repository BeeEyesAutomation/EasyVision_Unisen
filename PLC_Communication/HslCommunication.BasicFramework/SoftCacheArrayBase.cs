using HslCommunication.Core;

namespace HslCommunication.BasicFramework;

public abstract class SoftCacheArrayBase
{
	protected byte[] DataBytes = null;

	protected SimpleHybirdLock HybirdLock = new SimpleHybirdLock();

	public int ArrayLength { get; protected set; }

	public virtual void LoadFromBytes(byte[] dataSave)
	{
	}

	public byte[] GetAllData()
	{
		byte[] array = new byte[DataBytes.Length];
		DataBytes.CopyTo(array, 0);
		return array;
	}
}
