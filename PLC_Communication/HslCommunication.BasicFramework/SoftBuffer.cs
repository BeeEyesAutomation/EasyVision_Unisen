using System;
using System.IO;
using HslCommunication.Core;

namespace HslCommunication.BasicFramework;

public class SoftBuffer : IDisposable
{
	private int capacity = 10;

	private byte[] buffer;

	private SimpleHybirdLock hybirdLock;

	private IByteTransform byteTransform;

	private bool isBoolReverseByWord = false;

	private bool disposedValue = false;

	public IByteTransform ByteTransform
	{
		get
		{
			return byteTransform;
		}
		set
		{
			byteTransform = value;
		}
	}

	public bool IsBoolReverseByWord
	{
		get
		{
			return isBoolReverseByWord;
		}
		set
		{
			isBoolReverseByWord = value;
		}
	}

	public int Capacity => capacity;

	public SoftBuffer()
	{
		buffer = new byte[capacity];
		hybirdLock = new SimpleHybirdLock();
		byteTransform = new RegularByteTransform();
	}

	public SoftBuffer(int capacity)
	{
		buffer = new byte[capacity];
		this.capacity = capacity;
		hybirdLock = new SimpleHybirdLock();
		byteTransform = new RegularByteTransform();
	}

	public void SetBool(bool value, int destIndex)
	{
		SetBool(new bool[1] { value }, destIndex);
	}

	public void SetBool(bool[] value, int destIndex)
	{
		if (value == null)
		{
			return;
		}
		try
		{
			hybirdLock.Enter();
			for (int i = 0; i < value.Length; i++)
			{
				int num = (destIndex + i) / 8;
				int offset = (destIndex + i) % 8;
				if (isBoolReverseByWord)
				{
					num = ((num % 2 != 0) ? (num - 1) : (num + 1));
				}
				if (value[i])
				{
					buffer[num] |= getOrByte(offset);
				}
				else
				{
					buffer[num] &= getAndByte(offset);
				}
			}
			hybirdLock.Leave();
		}
		catch
		{
			hybirdLock.Leave();
			throw;
		}
	}

	public bool GetBool(int destIndex)
	{
		return GetBool(destIndex, 1)[0];
	}

	public bool[] GetBool(int destIndex, int length)
	{
		bool[] array = new bool[length];
		try
		{
			hybirdLock.Enter();
			for (int i = 0; i < length; i++)
			{
				int num = (destIndex + i) / 8;
				int offset = (destIndex + i) % 8;
				if (isBoolReverseByWord)
				{
					num = ((num % 2 != 0) ? (num - 1) : (num + 1));
				}
				array[i] = (buffer[num] & getOrByte(offset)) == getOrByte(offset);
			}
			hybirdLock.Leave();
		}
		catch
		{
			hybirdLock.Leave();
			throw;
		}
		return array;
	}

	private byte getAndByte(int offset)
	{
		if (1 == 0)
		{
		}
		byte result = offset switch
		{
			0 => 254, 
			1 => 253, 
			2 => 251, 
			3 => 247, 
			4 => 239, 
			5 => 223, 
			6 => 191, 
			7 => 127, 
			_ => byte.MaxValue, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private byte getOrByte(int offset)
	{
		if (1 == 0)
		{
		}
		byte result = offset switch
		{
			0 => 1, 
			1 => 2, 
			2 => 4, 
			3 => 8, 
			4 => 16, 
			5 => 32, 
			6 => 64, 
			7 => 128, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public void SetBytes(byte[] data, int destIndex)
	{
		if (destIndex < capacity && destIndex >= 0 && data != null)
		{
			hybirdLock.Enter();
			if (data.Length + destIndex > buffer.Length)
			{
				Array.Copy(data, 0, buffer, destIndex, buffer.Length - destIndex);
			}
			else
			{
				data.CopyTo(buffer, destIndex);
			}
			hybirdLock.Leave();
		}
	}

	public void SetBytes(byte[] data, int destIndex, int length)
	{
		if (destIndex < capacity && destIndex >= 0 && data != null)
		{
			if (length > data.Length)
			{
				length = data.Length;
			}
			hybirdLock.Enter();
			if (length + destIndex > buffer.Length)
			{
				Array.Copy(data, 0, buffer, destIndex, buffer.Length - destIndex);
			}
			else
			{
				Array.Copy(data, 0, buffer, destIndex, length);
			}
			hybirdLock.Leave();
		}
	}

	public void SetBytes(byte[] data, int sourceIndex, int destIndex, int length)
	{
		if (destIndex < capacity && destIndex >= 0 && data != null)
		{
			if (length > data.Length)
			{
				length = data.Length;
			}
			hybirdLock.Enter();
			Array.Copy(data, sourceIndex, buffer, destIndex, length);
			hybirdLock.Leave();
		}
	}

	public byte[] GetBytes(int index, int length)
	{
		byte[] array = new byte[length];
		if (length > 0)
		{
			hybirdLock.Enter();
			if (index >= 0 && index + length <= buffer.Length)
			{
				Array.Copy(buffer, index, array, 0, length);
			}
			hybirdLock.Leave();
		}
		return array;
	}

	public byte[] GetBytes()
	{
		return GetBytes(0, capacity);
	}

	public void SetValue(byte value, int index)
	{
		SetBytes(new byte[1] { value }, index);
	}

	public void SetValue(short[] values, int index)
	{
		SetBytes(byteTransform.TransByte(values), index);
	}

	public void SetValue(short value, int index)
	{
		SetValue(new short[1] { value }, index);
	}

	public void SetValue(ushort[] values, int index)
	{
		SetBytes(byteTransform.TransByte(values), index);
	}

	public void SetValue(ushort value, int index)
	{
		SetValue(new ushort[1] { value }, index);
	}

	public void SetValue(int[] values, int index)
	{
		SetBytes(byteTransform.TransByte(values), index);
	}

	public void SetValue(int value, int index)
	{
		SetValue(new int[1] { value }, index);
	}

	public void SetValue(uint[] values, int index)
	{
		SetBytes(byteTransform.TransByte(values), index);
	}

	public void SetValue(uint value, int index)
	{
		SetValue(new uint[1] { value }, index);
	}

	public void SetValue(float[] values, int index)
	{
		SetBytes(byteTransform.TransByte(values), index);
	}

	public void SetValue(float value, int index)
	{
		SetValue(new float[1] { value }, index);
	}

	public void SetValue(long[] values, int index)
	{
		SetBytes(byteTransform.TransByte(values), index);
	}

	public void SetValue(long value, int index)
	{
		SetValue(new long[1] { value }, index);
	}

	public void SetValue(ulong[] values, int index)
	{
		SetBytes(byteTransform.TransByte(values), index);
	}

	public void SetValue(ulong value, int index)
	{
		SetValue(new ulong[1] { value }, index);
	}

	public void SetValue(double[] values, int index)
	{
		SetBytes(byteTransform.TransByte(values), index);
	}

	public void SetValue(double value, int index)
	{
		SetValue(new double[1] { value }, index);
	}

	public byte GetByte(int index)
	{
		return GetBytes(index, 1)[0];
	}

	public short[] GetInt16(int index, int length)
	{
		return byteTransform.TransInt16(GetBytes(index, length * 2), 0, length);
	}

	public short GetInt16(int index)
	{
		return GetInt16(index, 1)[0];
	}

	public ushort[] GetUInt16(int index, int length)
	{
		return byteTransform.TransUInt16(GetBytes(index, length * 2), 0, length);
	}

	public ushort GetUInt16(int index)
	{
		return GetUInt16(index, 1)[0];
	}

	public int[] GetInt32(int index, int length)
	{
		return byteTransform.TransInt32(GetBytes(index, length * 4), 0, length);
	}

	public int GetInt32(int index)
	{
		return GetInt32(index, 1)[0];
	}

	public uint[] GetUInt32(int index, int length)
	{
		return byteTransform.TransUInt32(GetBytes(index, length * 4), 0, length);
	}

	public uint GetUInt32(int index)
	{
		return GetUInt32(index, 1)[0];
	}

	public float[] GetSingle(int index, int length)
	{
		return byteTransform.TransSingle(GetBytes(index, length * 4), 0, length);
	}

	public float GetSingle(int index)
	{
		return GetSingle(index, 1)[0];
	}

	public long[] GetInt64(int index, int length)
	{
		return byteTransform.TransInt64(GetBytes(index, length * 8), 0, length);
	}

	public long GetInt64(int index)
	{
		return GetInt64(index, 1)[0];
	}

	public ulong[] GetUInt64(int index, int length)
	{
		return byteTransform.TransUInt64(GetBytes(index, length * 8), 0, length);
	}

	public ulong GetUInt64(int index)
	{
		return GetUInt64(index, 1)[0];
	}

	public double[] GetDouble(int index, int length)
	{
		return byteTransform.TransDouble(GetBytes(index, length * 8), 0, length);
	}

	public double GetDouble(int index)
	{
		return GetDouble(index, 1)[0];
	}

	public T GetCustomer<T>(int index) where T : IDataTransfer, new()
	{
		T result = new T();
		byte[] bytes = GetBytes(index, result.ReadCount);
		result.ParseSource(bytes);
		return result;
	}

	public void SetCustomer<T>(T data, int index) where T : IDataTransfer, new()
	{
		SetBytes(data.ToSource(), index);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				hybirdLock?.Dispose();
				buffer = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public static MemoryStream ToMemoryStream(params SoftBuffer[] buffers)
	{
		MemoryStream memoryStream = new MemoryStream();
		if (buffers == null || buffers.Length == 0)
		{
			return memoryStream;
		}
		foreach (SoftBuffer softBuffer in buffers)
		{
			if (softBuffer != null && softBuffer.Capacity > 0)
			{
				memoryStream.Write(softBuffer.GetBytes());
			}
		}
		return memoryStream;
	}

	public static void LoadFromBuffer(byte[] source, params SoftBuffer[] buffers)
	{
		if (buffers == null || buffers.Length == 0)
		{
			return;
		}
		int num = 0;
		foreach (SoftBuffer softBuffer in buffers)
		{
			if (num >= source.Length || num + softBuffer.capacity > source.Length)
			{
				break;
			}
			if (softBuffer != null && softBuffer.Capacity > 0)
			{
				softBuffer.SetBytes(source.SelectMiddle(num, softBuffer.Capacity), 0);
				num += softBuffer.Capacity;
			}
		}
	}

	public override string ToString()
	{
		return $"SoftBuffer[{capacity}][{ByteTransform}]";
	}
}
