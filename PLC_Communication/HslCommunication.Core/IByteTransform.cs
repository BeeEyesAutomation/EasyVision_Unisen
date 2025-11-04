using System.Text;

namespace HslCommunication.Core;

public interface IByteTransform
{
	DataFormat DataFormat { get; set; }

	bool IsStringReverseByteWord { get; set; }

	bool TransBool(byte[] buffer, int index);

	bool[] TransBool(byte[] buffer, int index, int length);

	byte TransByte(byte[] buffer, int index);

	byte[] TransByte(byte[] buffer, int index, int length);

	short TransInt16(byte[] buffer, int index);

	short[] TransInt16(byte[] buffer, int index, int length);

	short[,] TransInt16(byte[] buffer, int index, int row, int col);

	ushort TransUInt16(byte[] buffer, int index);

	ushort[] TransUInt16(byte[] buffer, int index, int length);

	ushort[,] TransUInt16(byte[] buffer, int index, int row, int col);

	int TransInt32(byte[] buffer, int index);

	int[] TransInt32(byte[] buffer, int index, int length);

	int[,] TransInt32(byte[] buffer, int index, int row, int col);

	uint TransUInt32(byte[] buffer, int index);

	uint[] TransUInt32(byte[] buffer, int index, int length);

	uint[,] TransUInt32(byte[] buffer, int index, int row, int col);

	long TransInt64(byte[] buffer, int index);

	long[] TransInt64(byte[] buffer, int index, int length);

	long[,] TransInt64(byte[] buffer, int index, int row, int col);

	ulong TransUInt64(byte[] buffer, int index);

	ulong[] TransUInt64(byte[] buffer, int index, int length);

	ulong[,] TransUInt64(byte[] buffer, int index, int row, int col);

	float TransSingle(byte[] buffer, int index);

	float[] TransSingle(byte[] buffer, int index, int length);

	float[,] TransSingle(byte[] buffer, int index, int row, int col);

	double TransDouble(byte[] buffer, int index);

	double[] TransDouble(byte[] buffer, int index, int length);

	double[,] TransDouble(byte[] buffer, int index, int row, int col);

	string TransString(byte[] buffer, Encoding encoding);

	string TransString(byte[] buffer, int index, int length, Encoding encoding);

	byte[] TransByte(bool value);

	byte[] TransByte(bool[] values);

	byte[] TransByte(byte value);

	byte[] TransByte(short value);

	byte[] TransByte(short[] values);

	byte[] TransByte(ushort value);

	byte[] TransByte(ushort[] values);

	byte[] TransByte(int value);

	byte[] TransByte(int[] values);

	byte[] TransByte(uint value);

	byte[] TransByte(uint[] values);

	byte[] TransByte(long value);

	byte[] TransByte(long[] values);

	byte[] TransByte(ulong value);

	byte[] TransByte(ulong[] values);

	byte[] TransByte(float value);

	byte[] TransByte(float[] values);

	byte[] TransByte(double value);

	byte[] TransByte(double[] values);

	byte[] TransByte(string value, Encoding encoding);

	byte[] TransByte(string value, int length, Encoding encoding);

	IByteTransform CreateByDateFormat(DataFormat dataFormat);
}
