using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Device;

public class DeviceCommunication : BinaryCommunication, IReadWriteDevice, IReadWriteNet, IDisposable
{
	private bool disposedValue = false;

	private IByteTransform byteTransform = new RegularByteTransform();

	protected ushort WordLength { get; set; } = 1;

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

	protected virtual ushort GetWordLength(string address, int length, int dataTypeLength)
	{
		if (WordLength == 0)
		{
			int num = length * dataTypeLength * 2 / 4;
			return (ushort)((num == 0) ? 1 : ((ushort)num));
		}
		return (ushort)(WordLength * length * dataTypeLength);
	}

	[HslMqttApi("ReadByteArray", "")]
	public virtual OperateResult<byte[]> Read(string address, ushort length)
	{
		return new OperateResult<byte[]>(StringResources.Language.NotSupportedFunction);
	}

	[HslMqttApi("WriteByteArray", "")]
	public virtual OperateResult Write(string address, byte[] value)
	{
		return new OperateResult(StringResources.Language.NotSupportedFunction);
	}

	[HslMqttApi("ReadBoolArray", "")]
	public virtual OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return new OperateResult<bool[]>(StringResources.Language.NotSupportedFunction);
	}

	[HslMqttApi("ReadBool", "")]
	public virtual OperateResult<bool> ReadBool(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadBool(address, 1));
	}

	[HslMqttApi("WriteBoolArray", "")]
	public virtual OperateResult Write(string address, bool[] value)
	{
		return new OperateResult(StringResources.Language.NotSupportedFunction);
	}

	[HslMqttApi("WriteBool", "")]
	public virtual OperateResult Write(string address, bool value)
	{
		return Write(address, new bool[1] { value });
	}

	public OperateResult<T> ReadCustomer<T>(string address) where T : IDataTransfer, new()
	{
		return ReadWriteNetHelper.ReadCustomer<T>(this, address);
	}

	public OperateResult<T> ReadCustomer<T>(string address, T obj) where T : IDataTransfer, new()
	{
		return ReadWriteNetHelper.ReadCustomer(this, address, obj);
	}

	public OperateResult WriteCustomer<T>(string address, T data) where T : IDataTransfer, new()
	{
		return ReadWriteNetHelper.WriteCustomer(this, address, data);
	}

	public virtual OperateResult<T> Read<T>() where T : class, new()
	{
		return HslReflectionHelper.Read<T>(this);
	}

	public virtual OperateResult Write<T>(T data) where T : class, new()
	{
		return HslReflectionHelper.Write(data, this);
	}

	public virtual OperateResult<T> ReadStruct<T>(string address, ushort length) where T : class, new()
	{
		return ReadWriteNetHelper.ReadStruct<T>(this, address, length, ByteTransform);
	}

	[HslMqttApi("ReadInt16", "")]
	public OperateResult<short> ReadInt16(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadInt16(address, 1));
	}

	[HslMqttApi("ReadInt16Array", "")]
	public virtual OperateResult<short[]> ReadInt16(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 1)), (byte[] m) => ByteTransform.TransInt16(m, 0, length));
	}

	[HslMqttApi("ReadUInt16", "")]
	public OperateResult<ushort> ReadUInt16(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadUInt16(address, 1));
	}

	[HslMqttApi("ReadUInt16Array", "")]
	public virtual OperateResult<ushort[]> ReadUInt16(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 1)), (byte[] m) => ByteTransform.TransUInt16(m, 0, length));
	}

	[HslMqttApi("ReadInt32", "")]
	public OperateResult<int> ReadInt32(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadInt32(address, 1));
	}

	[HslMqttApi("ReadInt32Array", "")]
	public virtual OperateResult<int[]> ReadInt32(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 2)), (byte[] m) => ByteTransform.TransInt32(m, 0, length));
	}

	[HslMqttApi("ReadUInt32", "")]
	public OperateResult<uint> ReadUInt32(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadUInt32(address, 1));
	}

	[HslMqttApi("ReadUInt32Array", "")]
	public virtual OperateResult<uint[]> ReadUInt32(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 2)), (byte[] m) => ByteTransform.TransUInt32(m, 0, length));
	}

	[HslMqttApi("ReadFloat", "")]
	public OperateResult<float> ReadFloat(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadFloat(address, 1));
	}

	[HslMqttApi("ReadFloatArray", "")]
	public virtual OperateResult<float[]> ReadFloat(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 2)), (byte[] m) => ByteTransform.TransSingle(m, 0, length));
	}

	[HslMqttApi("ReadInt64", "")]
	public OperateResult<long> ReadInt64(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadInt64(address, 1));
	}

	[HslMqttApi("ReadInt64Array", "")]
	public virtual OperateResult<long[]> ReadInt64(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 4)), (byte[] m) => ByteTransform.TransInt64(m, 0, length));
	}

	[HslMqttApi("ReadUInt64", "")]
	public OperateResult<ulong> ReadUInt64(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadUInt64(address, 1));
	}

	[HslMqttApi("ReadUInt64Array", "")]
	public virtual OperateResult<ulong[]> ReadUInt64(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 4)), (byte[] m) => ByteTransform.TransUInt64(m, 0, length));
	}

	[HslMqttApi("ReadDouble", "")]
	public OperateResult<double> ReadDouble(string address)
	{
		return ByteTransformHelper.GetResultFromArray(ReadDouble(address, 1));
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public virtual OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, GetWordLength(address, length, 4)), (byte[] m) => ByteTransform.TransDouble(m, 0, length));
	}

	[HslMqttApi("ReadString", "")]
	public virtual OperateResult<string> ReadString(string address, ushort length)
	{
		return ReadString(address, length, Encoding.ASCII);
	}

	public virtual OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => ByteTransform.TransString(m, 0, m.Length, encoding));
	}

	[HslMqttApi("WriteInt16Array", "")]
	public virtual OperateResult Write(string address, short[] values)
	{
		return Write(address, ByteTransform.TransByte(values));
	}

	[HslMqttApi("WriteInt16", "")]
	public virtual OperateResult Write(string address, short value)
	{
		return Write(address, new short[1] { value });
	}

	[HslMqttApi("WriteUInt16Array", "")]
	public virtual OperateResult Write(string address, ushort[] values)
	{
		return Write(address, ByteTransform.TransByte(values));
	}

	[HslMqttApi("WriteUInt16", "")]
	public virtual OperateResult Write(string address, ushort value)
	{
		return Write(address, new ushort[1] { value });
	}

	[HslMqttApi("WriteInt32Array", "")]
	public virtual OperateResult Write(string address, int[] values)
	{
		return Write(address, ByteTransform.TransByte(values));
	}

	[HslMqttApi("WriteInt32", "")]
	public virtual OperateResult Write(string address, int value)
	{
		return Write(address, new int[1] { value });
	}

	[HslMqttApi("WriteUInt32Array", "")]
	public virtual OperateResult Write(string address, uint[] values)
	{
		return Write(address, ByteTransform.TransByte(values));
	}

	[HslMqttApi("WriteUInt32", "")]
	public virtual OperateResult Write(string address, uint value)
	{
		return Write(address, new uint[1] { value });
	}

	[HslMqttApi("WriteFloatArray", "")]
	public virtual OperateResult Write(string address, float[] values)
	{
		return Write(address, ByteTransform.TransByte(values));
	}

	[HslMqttApi("WriteFloat", "")]
	public virtual OperateResult Write(string address, float value)
	{
		return Write(address, new float[1] { value });
	}

	[HslMqttApi("WriteInt64Array", "")]
	public virtual OperateResult Write(string address, long[] values)
	{
		return Write(address, ByteTransform.TransByte(values));
	}

	[HslMqttApi("WriteInt64", "")]
	public virtual OperateResult Write(string address, long value)
	{
		return Write(address, new long[1] { value });
	}

	[HslMqttApi("WriteUInt64Array", "")]
	public virtual OperateResult Write(string address, ulong[] values)
	{
		return Write(address, ByteTransform.TransByte(values));
	}

	[HslMqttApi("WriteUInt64", "")]
	public virtual OperateResult Write(string address, ulong value)
	{
		return Write(address, new ulong[1] { value });
	}

	[HslMqttApi("WriteDoubleArray", "")]
	public virtual OperateResult Write(string address, double[] values)
	{
		return Write(address, ByteTransform.TransByte(values));
	}

	[HslMqttApi("WriteDouble", "")]
	public virtual OperateResult Write(string address, double value)
	{
		return Write(address, new double[1] { value });
	}

	[HslMqttApi("WriteString", "")]
	public virtual OperateResult Write(string address, string value)
	{
		return Write(address, value, Encoding.ASCII);
	}

	public virtual OperateResult Write(string address, string value, int length)
	{
		return Write(address, value, length, Encoding.ASCII);
	}

	public virtual OperateResult Write(string address, string value, Encoding encoding)
	{
		byte[] array = ByteTransform.TransByte(value, encoding);
		if (WordLength == 1)
		{
			array = SoftBasic.ArrayExpandToLengthEven(array);
		}
		return Write(address, array);
	}

	public virtual OperateResult Write(string address, string value, int length, Encoding encoding)
	{
		byte[] data = ByteTransform.TransByte(value, encoding);
		if (WordLength == 1)
		{
			data = SoftBasic.ArrayExpandToLengthEven(data);
		}
		data = SoftBasic.ArrayExpandToLength(data, length);
		return Write(address, data);
	}

	[HslMqttApi("WaitBool", "")]
	public OperateResult<TimeSpan> Wait(string address, bool waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return ReadWriteNetHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
	}

	[HslMqttApi("WaitInt16", "")]
	public OperateResult<TimeSpan> Wait(string address, short waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return ReadWriteNetHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
	}

	[HslMqttApi("WaitUInt16", "")]
	public OperateResult<TimeSpan> Wait(string address, ushort waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return ReadWriteNetHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
	}

	[HslMqttApi("WaitInt32", "")]
	public OperateResult<TimeSpan> Wait(string address, int waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return ReadWriteNetHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
	}

	[HslMqttApi("WaitUInt32", "")]
	public OperateResult<TimeSpan> Wait(string address, uint waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return ReadWriteNetHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
	}

	[HslMqttApi("WaitInt64", "")]
	public OperateResult<TimeSpan> Wait(string address, long waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return ReadWriteNetHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
	}

	[HslMqttApi("WaitUInt64", "")]
	public OperateResult<TimeSpan> Wait(string address, ulong waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return ReadWriteNetHelper.Wait(this, address, waitValue, readInterval, waitTimeout);
	}

	public async Task<OperateResult<TimeSpan>> WaitAsync(string address, bool waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return await ReadWriteNetHelper.WaitAsync(this, address, waitValue, readInterval, waitTimeout);
	}

	public async Task<OperateResult<TimeSpan>> WaitAsync(string address, short waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return await ReadWriteNetHelper.WaitAsync(this, address, waitValue, readInterval, waitTimeout);
	}

	public async Task<OperateResult<TimeSpan>> WaitAsync(string address, ushort waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return await ReadWriteNetHelper.WaitAsync(this, address, waitValue, readInterval, waitTimeout);
	}

	public async Task<OperateResult<TimeSpan>> WaitAsync(string address, int waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return await ReadWriteNetHelper.WaitAsync(this, address, waitValue, readInterval, waitTimeout);
	}

	public async Task<OperateResult<TimeSpan>> WaitAsync(string address, uint waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return await ReadWriteNetHelper.WaitAsync(this, address, waitValue, readInterval, waitTimeout);
	}

	public async Task<OperateResult<TimeSpan>> WaitAsync(string address, long waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return await ReadWriteNetHelper.WaitAsync(this, address, waitValue, readInterval, waitTimeout);
	}

	public async Task<OperateResult<TimeSpan>> WaitAsync(string address, ulong waitValue, int readInterval = 100, int waitTimeout = -1)
	{
		return await ReadWriteNetHelper.WaitAsync(this, address, waitValue, readInterval, waitTimeout);
	}

	public virtual async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		return await Task.Run(() => Read(address, length));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await Task.Run(() => Write(address, value));
	}

	public virtual async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await Task.Run(() => ReadBool(address, length));
	}

	public virtual async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadBoolAsync(address, 1));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await Task.Run(() => Write(address, value));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await WriteAsync(address, new bool[1] { value });
	}

	public async Task<OperateResult<T>> ReadCustomerAsync<T>(string address) where T : IDataTransfer, new()
	{
		return await ReadWriteNetHelper.ReadCustomerAsync<T>(this, address);
	}

	public async Task<OperateResult<T>> ReadCustomerAsync<T>(string address, T obj) where T : IDataTransfer, new()
	{
		return await ReadWriteNetHelper.ReadCustomerAsync(this, address, obj);
	}

	public async Task<OperateResult> WriteCustomerAsync<T>(string address, T data) where T : IDataTransfer, new()
	{
		return await ReadWriteNetHelper.WriteCustomerAsync(this, address, data);
	}

	public virtual async Task<OperateResult<T>> ReadAsync<T>() where T : class, new()
	{
		return await HslReflectionHelper.ReadAsync<T>(this);
	}

	public virtual async Task<OperateResult> WriteAsync<T>(T data) where T : class, new()
	{
		return await HslReflectionHelper.WriteAsync(data, this);
	}

	public virtual async Task<OperateResult<T>> ReadStructAsync<T>(string address, ushort length) where T : class, new()
	{
		return await ReadWriteNetHelper.ReadStructAsync<T>(this, address, length, ByteTransform);
	}

	public async Task<OperateResult<short>> ReadInt16Async(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadInt16Async(address, 1));
	}

	public virtual async Task<OperateResult<short[]>> ReadInt16Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 1)), (byte[] m) => ByteTransform.TransInt16(m, 0, length));
	}

	public async Task<OperateResult<ushort>> ReadUInt16Async(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadUInt16Async(address, 1));
	}

	public virtual async Task<OperateResult<ushort[]>> ReadUInt16Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 1)), (byte[] m) => ByteTransform.TransUInt16(m, 0, length));
	}

	public async Task<OperateResult<int>> ReadInt32Async(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadInt32Async(address, 1));
	}

	public virtual async Task<OperateResult<int[]>> ReadInt32Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 2)), (byte[] m) => ByteTransform.TransInt32(m, 0, length));
	}

	public async Task<OperateResult<uint>> ReadUInt32Async(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadUInt32Async(address, 1));
	}

	public virtual async Task<OperateResult<uint[]>> ReadUInt32Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 2)), (byte[] m) => ByteTransform.TransUInt32(m, 0, length));
	}

	public async Task<OperateResult<float>> ReadFloatAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadFloatAsync(address, 1));
	}

	public virtual async Task<OperateResult<float[]>> ReadFloatAsync(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 2)), (byte[] m) => ByteTransform.TransSingle(m, 0, length));
	}

	public async Task<OperateResult<long>> ReadInt64Async(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadInt64Async(address, 1));
	}

	public virtual async Task<OperateResult<long[]>> ReadInt64Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 4)), (byte[] m) => ByteTransform.TransInt64(m, 0, length));
	}

	public async Task<OperateResult<ulong>> ReadUInt64Async(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadUInt64Async(address, 1));
	}

	public virtual async Task<OperateResult<ulong[]>> ReadUInt64Async(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 4)), (byte[] m) => ByteTransform.TransUInt64(m, 0, length));
	}

	public async Task<OperateResult<double>> ReadDoubleAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadDoubleAsync(address, 1));
	}

	public virtual async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, GetWordLength(address, length, 4)), (byte[] m) => ByteTransform.TransDouble(m, 0, length));
	}

	public virtual async Task<OperateResult<string>> ReadStringAsync(string address, ushort length)
	{
		return await ReadStringAsync(address, length, Encoding.ASCII);
	}

	public virtual async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => ByteTransform.TransString(m, 0, m.Length, encoding));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, short[] values)
	{
		return await WriteAsync(address, ByteTransform.TransByte(values));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, short value)
	{
		return await WriteAsync(address, new short[1] { value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, ushort[] values)
	{
		return await WriteAsync(address, ByteTransform.TransByte(values));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, ushort value)
	{
		return await WriteAsync(address, new ushort[1] { value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, int[] values)
	{
		return await WriteAsync(address, ByteTransform.TransByte(values));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, int value)
	{
		return await WriteAsync(address, new int[1] { value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, uint[] values)
	{
		return await WriteAsync(address, ByteTransform.TransByte(values));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, uint value)
	{
		return await WriteAsync(address, new uint[1] { value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, float[] values)
	{
		return await WriteAsync(address, ByteTransform.TransByte(values));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, float value)
	{
		return await WriteAsync(address, new float[1] { value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, long[] values)
	{
		return await WriteAsync(address, ByteTransform.TransByte(values));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, long value)
	{
		return await WriteAsync(address, new long[1] { value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, ulong[] values)
	{
		return await WriteAsync(address, ByteTransform.TransByte(values));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, ulong value)
	{
		return await WriteAsync(address, new ulong[1] { value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await WriteAsync(address, ByteTransform.TransByte(values));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, double value)
	{
		return await WriteAsync(address, new double[1] { value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value)
	{
		return await WriteAsync(address, value, Encoding.ASCII);
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value, Encoding encoding)
	{
		byte[] temp = ByteTransform.TransByte(value, encoding);
		if (WordLength == 1)
		{
			temp = SoftBasic.ArrayExpandToLengthEven(temp);
		}
		return await WriteAsync(address, temp);
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value, int length)
	{
		return await WriteAsync(address, value, length, Encoding.ASCII);
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value, int length, Encoding encoding)
	{
		byte[] temp2 = ByteTransform.TransByte(value, encoding);
		if (WordLength == 1)
		{
			temp2 = SoftBasic.ArrayExpandToLengthEven(temp2);
		}
		temp2 = SoftBasic.ArrayExpandToLength(temp2, length);
		return await WriteAsync(address, temp2);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			CommunicationPipe?.CloseCommunication();
			CommunicationPipe?.Dispose();
		}
	}

	public void Dispose()
	{
		if (!disposedValue)
		{
			Dispose(disposing: true);
			disposedValue = true;
		}
	}

	public override string ToString()
	{
		return $"DeviceCommunication<{byteTransform}>{{{CommunicationPipe}}}";
	}
}
