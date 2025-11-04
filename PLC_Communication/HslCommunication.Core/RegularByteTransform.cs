using System;
using System.Text;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core;

public class RegularByteTransform : IByteTransform
{
	public DataFormat DataFormat { get; set; }

	public bool IsStringReverseByteWord { get; set; }

	public RegularByteTransform()
	{
		DataFormat = DataFormat.DCBA;
	}

	public RegularByteTransform(DataFormat dataFormat)
	{
		DataFormat = dataFormat;
	}

	public virtual bool TransBool(byte[] buffer, int index)
	{
		return buffer.GetBoolByIndex(index);
	}

	public virtual bool[] TransBool(byte[] buffer, int index, int length)
	{
		bool[] array = new bool[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = buffer.GetBoolByIndex(i + index);
		}
		return array;
	}

	public virtual byte TransByte(byte[] buffer, int index)
	{
		return buffer[index];
	}

	public virtual byte[] TransByte(byte[] buffer, int index, int length)
	{
		byte[] array = new byte[length];
		Array.Copy(buffer, index, array, 0, length);
		return array;
	}

	public virtual short TransInt16(byte[] buffer, int index)
	{
		return BitConverter.ToInt16(ByteTransDataFormat2(buffer, index), 0);
	}

	public virtual short[] TransInt16(byte[] buffer, int index, int length)
	{
		short[] array = new short[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = TransInt16(buffer, index + 2 * i);
		}
		return array;
	}

	public short[,] TransInt16(byte[] buffer, int index, int row, int col)
	{
		return HslHelper.CreateTwoArrayFromOneArray(TransInt16(buffer, index, row * col), row, col);
	}

	public virtual ushort TransUInt16(byte[] buffer, int index)
	{
		return BitConverter.ToUInt16(ByteTransDataFormat2(buffer, index), 0);
	}

	public virtual ushort[] TransUInt16(byte[] buffer, int index, int length)
	{
		ushort[] array = new ushort[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = TransUInt16(buffer, index + 2 * i);
		}
		return array;
	}

	public ushort[,] TransUInt16(byte[] buffer, int index, int row, int col)
	{
		return HslHelper.CreateTwoArrayFromOneArray(TransUInt16(buffer, index, row * col), row, col);
	}

	public virtual int TransInt32(byte[] buffer, int index)
	{
		return BitConverter.ToInt32(ByteTransDataFormat4(buffer, index), 0);
	}

	public virtual int[] TransInt32(byte[] buffer, int index, int length)
	{
		int[] array = new int[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = TransInt32(buffer, index + 4 * i);
		}
		return array;
	}

	public int[,] TransInt32(byte[] buffer, int index, int row, int col)
	{
		return HslHelper.CreateTwoArrayFromOneArray(TransInt32(buffer, index, row * col), row, col);
	}

	public virtual uint TransUInt32(byte[] buffer, int index)
	{
		return BitConverter.ToUInt32(ByteTransDataFormat4(buffer, index), 0);
	}

	public virtual uint[] TransUInt32(byte[] buffer, int index, int length)
	{
		uint[] array = new uint[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = TransUInt32(buffer, index + 4 * i);
		}
		return array;
	}

	public uint[,] TransUInt32(byte[] buffer, int index, int row, int col)
	{
		return HslHelper.CreateTwoArrayFromOneArray(TransUInt32(buffer, index, row * col), row, col);
	}

	public virtual long TransInt64(byte[] buffer, int index)
	{
		return BitConverter.ToInt64(ByteTransDataFormat8(buffer, index), 0);
	}

	public virtual long[] TransInt64(byte[] buffer, int index, int length)
	{
		long[] array = new long[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = TransInt64(buffer, index + 8 * i);
		}
		return array;
	}

	public long[,] TransInt64(byte[] buffer, int index, int row, int col)
	{
		return HslHelper.CreateTwoArrayFromOneArray(TransInt64(buffer, index, row * col), row, col);
	}

	public virtual ulong TransUInt64(byte[] buffer, int index)
	{
		return BitConverter.ToUInt64(ByteTransDataFormat8(buffer, index), 0);
	}

	public virtual ulong[] TransUInt64(byte[] buffer, int index, int length)
	{
		ulong[] array = new ulong[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = TransUInt64(buffer, index + 8 * i);
		}
		return array;
	}

	public ulong[,] TransUInt64(byte[] buffer, int index, int row, int col)
	{
		return HslHelper.CreateTwoArrayFromOneArray(TransUInt64(buffer, index, row * col), row, col);
	}

	public virtual float TransSingle(byte[] buffer, int index)
	{
		return BitConverter.ToSingle(ByteTransDataFormat4(buffer, index), 0);
	}

	public virtual float[] TransSingle(byte[] buffer, int index, int length)
	{
		float[] array = new float[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = TransSingle(buffer, index + 4 * i);
		}
		return array;
	}

	public float[,] TransSingle(byte[] buffer, int index, int row, int col)
	{
		return HslHelper.CreateTwoArrayFromOneArray(TransSingle(buffer, index, row * col), row, col);
	}

	public virtual double TransDouble(byte[] buffer, int index)
	{
		return BitConverter.ToDouble(ByteTransDataFormat8(buffer, index), 0);
	}

	public virtual double[] TransDouble(byte[] buffer, int index, int length)
	{
		double[] array = new double[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = TransDouble(buffer, index + 8 * i);
		}
		return array;
	}

	public double[,] TransDouble(byte[] buffer, int index, int row, int col)
	{
		return HslHelper.CreateTwoArrayFromOneArray(TransDouble(buffer, index, row * col), row, col);
	}

	public virtual string TransString(byte[] buffer, int index, int length, Encoding encoding)
	{
		byte[] array = TransByte(buffer, index, length);
		if (IsStringReverseByteWord)
		{
			return encoding.GetString(SoftBasic.BytesReverseByWord(array));
		}
		return encoding.GetString(array);
	}

	public virtual string TransString(byte[] buffer, Encoding encoding)
	{
		return encoding.GetString(buffer);
	}

	public virtual byte[] TransByte(bool value)
	{
		return TransByte(new bool[1] { value });
	}

	public virtual byte[] TransByte(bool[] values)
	{
		return (values == null) ? null : SoftBasic.BoolArrayToByte(values);
	}

	public virtual byte[] TransByte(byte value)
	{
		return new byte[1] { value };
	}

	public virtual byte[] TransByte(short value)
	{
		return TransByte(new short[1] { value });
	}

	public virtual byte[] TransByte(short[] values)
	{
		if (values == null)
		{
			return null;
		}
		byte[] array = new byte[values.Length * 2];
		for (int i = 0; i < values.Length; i++)
		{
			ByteTransDataFormat2(BitConverter.GetBytes(values[i])).CopyTo(array, 2 * i);
		}
		return array;
	}

	public virtual byte[] TransByte(ushort value)
	{
		return TransByte(new ushort[1] { value });
	}

	public virtual byte[] TransByte(ushort[] values)
	{
		if (values == null)
		{
			return null;
		}
		byte[] array = new byte[values.Length * 2];
		for (int i = 0; i < values.Length; i++)
		{
			ByteTransDataFormat2(BitConverter.GetBytes(values[i])).CopyTo(array, 2 * i);
		}
		return array;
	}

	public virtual byte[] TransByte(int value)
	{
		return TransByte(new int[1] { value });
	}

	public virtual byte[] TransByte(int[] values)
	{
		if (values == null)
		{
			return null;
		}
		byte[] array = new byte[values.Length * 4];
		for (int i = 0; i < values.Length; i++)
		{
			ByteTransDataFormat4(BitConverter.GetBytes(values[i])).CopyTo(array, 4 * i);
		}
		return array;
	}

	public virtual byte[] TransByte(uint value)
	{
		return TransByte(new uint[1] { value });
	}

	public virtual byte[] TransByte(uint[] values)
	{
		if (values == null)
		{
			return null;
		}
		byte[] array = new byte[values.Length * 4];
		for (int i = 0; i < values.Length; i++)
		{
			ByteTransDataFormat4(BitConverter.GetBytes(values[i])).CopyTo(array, 4 * i);
		}
		return array;
	}

	public virtual byte[] TransByte(long value)
	{
		return TransByte(new long[1] { value });
	}

	public virtual byte[] TransByte(long[] values)
	{
		if (values == null)
		{
			return null;
		}
		byte[] array = new byte[values.Length * 8];
		for (int i = 0; i < values.Length; i++)
		{
			ByteTransDataFormat8(BitConverter.GetBytes(values[i])).CopyTo(array, 8 * i);
		}
		return array;
	}

	public virtual byte[] TransByte(ulong value)
	{
		return TransByte(new ulong[1] { value });
	}

	public virtual byte[] TransByte(ulong[] values)
	{
		if (values == null)
		{
			return null;
		}
		byte[] array = new byte[values.Length * 8];
		for (int i = 0; i < values.Length; i++)
		{
			ByteTransDataFormat8(BitConverter.GetBytes(values[i])).CopyTo(array, 8 * i);
		}
		return array;
	}

	public virtual byte[] TransByte(float value)
	{
		return TransByte(new float[1] { value });
	}

	public virtual byte[] TransByte(float[] values)
	{
		if (values == null)
		{
			return null;
		}
		byte[] array = new byte[values.Length * 4];
		for (int i = 0; i < values.Length; i++)
		{
			ByteTransDataFormat4(BitConverter.GetBytes(values[i])).CopyTo(array, 4 * i);
		}
		return array;
	}

	public virtual byte[] TransByte(double value)
	{
		return TransByte(new double[1] { value });
	}

	public virtual byte[] TransByte(double[] values)
	{
		if (values == null)
		{
			return null;
		}
		byte[] array = new byte[values.Length * 8];
		for (int i = 0; i < values.Length; i++)
		{
			ByteTransDataFormat8(BitConverter.GetBytes(values[i])).CopyTo(array, 8 * i);
		}
		return array;
	}

	public virtual byte[] TransByte(string value, Encoding encoding)
	{
		if (value == null)
		{
			return null;
		}
		byte[] bytes = encoding.GetBytes(value);
		return IsStringReverseByteWord ? SoftBasic.BytesReverseByWord(bytes) : bytes;
	}

	public virtual byte[] TransByte(string value, int length, Encoding encoding)
	{
		if (value == null)
		{
			return null;
		}
		byte[] bytes = encoding.GetBytes(value);
		return IsStringReverseByteWord ? SoftBasic.ArrayExpandToLength(SoftBasic.BytesReverseByWord(bytes), length) : SoftBasic.ArrayExpandToLength(bytes, length);
	}

	protected virtual byte[] ByteTransDataFormat2(byte[] value, int index = 0)
	{
		byte[] array = new byte[2];
		switch (DataFormat)
		{
		case DataFormat.ABCD:
		case DataFormat.CDAB:
			array[0] = value[index + 1];
			array[1] = value[index];
			break;
		case DataFormat.BADC:
		case DataFormat.DCBA:
			array[0] = value[index];
			array[1] = value[index + 1];
			break;
		}
		return array;
	}

	protected virtual byte[] ByteTransDataFormat4(byte[] value, int index = 0)
	{
		byte[] array = new byte[4];
		switch (DataFormat)
		{
		case DataFormat.ABCD:
			array[0] = value[index + 3];
			array[1] = value[index + 2];
			array[2] = value[index + 1];
			array[3] = value[index];
			break;
		case DataFormat.BADC:
			array[0] = value[index + 2];
			array[1] = value[index + 3];
			array[2] = value[index];
			array[3] = value[index + 1];
			break;
		case DataFormat.CDAB:
			array[0] = value[index + 1];
			array[1] = value[index];
			array[2] = value[index + 3];
			array[3] = value[index + 2];
			break;
		case DataFormat.DCBA:
			array[0] = value[index];
			array[1] = value[index + 1];
			array[2] = value[index + 2];
			array[3] = value[index + 3];
			break;
		}
		return array;
	}

	protected virtual byte[] ByteTransDataFormat8(byte[] value, int index = 0)
	{
		byte[] array = new byte[8];
		switch (DataFormat)
		{
		case DataFormat.ABCD:
			array[0] = value[index + 7];
			array[1] = value[index + 6];
			array[2] = value[index + 5];
			array[3] = value[index + 4];
			array[4] = value[index + 3];
			array[5] = value[index + 2];
			array[6] = value[index + 1];
			array[7] = value[index];
			break;
		case DataFormat.BADC:
			array[0] = value[index + 6];
			array[1] = value[index + 7];
			array[2] = value[index + 4];
			array[3] = value[index + 5];
			array[4] = value[index + 2];
			array[5] = value[index + 3];
			array[6] = value[index];
			array[7] = value[index + 1];
			break;
		case DataFormat.CDAB:
			array[0] = value[index + 1];
			array[1] = value[index];
			array[2] = value[index + 3];
			array[3] = value[index + 2];
			array[4] = value[index + 5];
			array[5] = value[index + 4];
			array[6] = value[index + 7];
			array[7] = value[index + 6];
			break;
		case DataFormat.DCBA:
			array[0] = value[index];
			array[1] = value[index + 1];
			array[2] = value[index + 2];
			array[3] = value[index + 3];
			array[4] = value[index + 4];
			array[5] = value[index + 5];
			array[6] = value[index + 6];
			array[7] = value[index + 7];
			break;
		}
		return array;
	}

	public virtual IByteTransform CreateByDateFormat(DataFormat dataFormat)
	{
		return new RegularByteTransform(dataFormat)
		{
			IsStringReverseByteWord = IsStringReverseByteWord
		};
	}

	public override string ToString()
	{
		return $"RegularByteTransform[{DataFormat}]";
	}
}
