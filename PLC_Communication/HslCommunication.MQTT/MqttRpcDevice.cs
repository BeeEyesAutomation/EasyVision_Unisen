using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;

namespace HslCommunication.MQTT;

public class MqttRpcDevice : MqttSyncClient, IReadWriteDevice, IReadWriteNet
{
	private IByteTransform byteTransform = new RegularByteTransform();

	private string deviceTopic;

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

	public string DeviceTopic
	{
		get
		{
			return deviceTopic;
		}
		set
		{
			deviceTopic = value;
		}
	}

	public MqttRpcDevice(MqttConnectionOptions options, string topic = null)
		: base(options)
	{
		deviceTopic = topic;
	}

	public MqttRpcDevice(string ipAddress, int port, string topic = null)
		: base(ipAddress, port)
	{
		deviceTopic = topic;
	}

	private string GetTopic(string topic)
	{
		if (string.IsNullOrEmpty(deviceTopic))
		{
			return topic;
		}
		if (deviceTopic.EndsWith("/"))
		{
			return deviceTopic + topic;
		}
		return deviceTopic + "/" + topic;
	}

	[HslMqttApi("ReadByteArray", "")]
	public virtual OperateResult<byte[]> Read(string address, ushort length)
	{
		return ReadRpc<byte[]>(GetTopic("ReadByteArray"), new { address, length });
	}

	[HslMqttApi("WriteByteArray", "")]
	public virtual OperateResult Write(string address, byte[] value)
	{
		return ReadRpc<string>(GetTopic("WriteByteArray"), new
		{
			address = address,
			value = value.ToHexString()
		});
	}

	[HslMqttApi("ReadBoolArray", "")]
	public virtual OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		return ReadRpc<bool[]>(GetTopic("ReadBoolArray"), new { address, length });
	}

	[HslMqttApi("ReadBool", "")]
	public virtual OperateResult<bool> ReadBool(string address)
	{
		return ReadRpc<bool>(GetTopic("ReadBool"), new { address });
	}

	[HslMqttApi("WriteBoolArray", "")]
	public virtual OperateResult Write(string address, bool[] value)
	{
		return ReadRpc<string>(GetTopic("WriteBoolArray"), new { address, value });
	}

	[HslMqttApi("WriteBool", "")]
	public virtual OperateResult Write(string address, bool value)
	{
		return ReadRpc<string>(GetTopic("WriteBool"), new { address, value });
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
		return ReadRpc<short>(GetTopic("ReadInt16"), new { address });
	}

	[HslMqttApi("ReadInt16Array", "")]
	public virtual OperateResult<short[]> ReadInt16(string address, ushort length)
	{
		return ReadRpc<short[]>(GetTopic("ReadInt16Array"), new { address, length });
	}

	[HslMqttApi("ReadUInt16", "")]
	public OperateResult<ushort> ReadUInt16(string address)
	{
		return ReadRpc<ushort>(GetTopic("ReadUInt16"), new { address });
	}

	[HslMqttApi("ReadUInt16Array", "")]
	public virtual OperateResult<ushort[]> ReadUInt16(string address, ushort length)
	{
		return ReadRpc<ushort[]>(GetTopic("ReadUInt16Array"), new { address, length });
	}

	[HslMqttApi("ReadInt32", "")]
	public OperateResult<int> ReadInt32(string address)
	{
		return ReadRpc<int>(GetTopic("ReadInt32"), new { address });
	}

	[HslMqttApi("ReadInt32Array", "")]
	public virtual OperateResult<int[]> ReadInt32(string address, ushort length)
	{
		return ReadRpc<int[]>(GetTopic("ReadInt32Array"), new { address, length });
	}

	[HslMqttApi("ReadUInt32", "")]
	public OperateResult<uint> ReadUInt32(string address)
	{
		return ReadRpc<uint>(GetTopic("ReadUInt32"), new { address });
	}

	[HslMqttApi("ReadUInt32Array", "")]
	public virtual OperateResult<uint[]> ReadUInt32(string address, ushort length)
	{
		return ReadRpc<uint[]>(GetTopic("ReadUInt32Array"), new { address, length });
	}

	[HslMqttApi("ReadFloat", "")]
	public OperateResult<float> ReadFloat(string address)
	{
		return ReadRpc<float>(GetTopic("ReadFloat"), new { address });
	}

	[HslMqttApi("ReadFloatArray", "")]
	public virtual OperateResult<float[]> ReadFloat(string address, ushort length)
	{
		return ReadRpc<float[]>(GetTopic("ReadFloatArray"), new { address, length });
	}

	[HslMqttApi("ReadInt64", "")]
	public OperateResult<long> ReadInt64(string address)
	{
		return ReadRpc<long>(GetTopic("ReadInt64"), new { address });
	}

	[HslMqttApi("ReadInt64Array", "")]
	public virtual OperateResult<long[]> ReadInt64(string address, ushort length)
	{
		return ReadRpc<long[]>(GetTopic("ReadInt64Array"), new { address, length });
	}

	[HslMqttApi("ReadUInt64", "")]
	public OperateResult<ulong> ReadUInt64(string address)
	{
		return ReadRpc<ulong>(GetTopic("ReadUInt64"), new { address });
	}

	[HslMqttApi("ReadUInt64Array", "")]
	public virtual OperateResult<ulong[]> ReadUInt64(string address, ushort length)
	{
		return ReadRpc<ulong[]>(GetTopic("ReadUInt64Array"), new { address, length });
	}

	[HslMqttApi("ReadDouble", "")]
	public OperateResult<double> ReadDouble(string address)
	{
		return ReadRpc<double>(GetTopic("ReadDouble"), new { address });
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public virtual OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		return ReadRpc<double[]>(GetTopic("ReadDoubleArray"), new { address, length });
	}

	[HslMqttApi("ReadString", "")]
	public virtual OperateResult<string> ReadString(string address, ushort length)
	{
		return ReadRpc<string>(GetTopic("ReadString"), new { address, length });
	}

	public virtual OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromBytes(Read(address, length), (byte[] m) => ByteTransform.TransString(m, 0, m.Length, encoding));
	}

	[HslMqttApi("WriteInt16Array", "")]
	public virtual OperateResult Write(string address, short[] values)
	{
		return ReadRpc<string>(GetTopic("WriteInt16Array"), new { address, values });
	}

	[HslMqttApi("WriteInt16", "")]
	public virtual OperateResult Write(string address, short value)
	{
		return ReadRpc<string>(GetTopic("WriteInt16"), new { address, value });
	}

	[HslMqttApi("WriteUInt16Array", "")]
	public virtual OperateResult Write(string address, ushort[] values)
	{
		return ReadRpc<string>(GetTopic("WriteUInt16Array"), new { address, values });
	}

	[HslMqttApi("WriteUInt16", "")]
	public virtual OperateResult Write(string address, ushort value)
	{
		return ReadRpc<string>(GetTopic("WriteUInt16"), new { address, value });
	}

	[HslMqttApi("WriteInt32Array", "")]
	public virtual OperateResult Write(string address, int[] values)
	{
		return ReadRpc<string>(GetTopic("WriteInt32Array"), new { address, values });
	}

	[HslMqttApi("WriteInt32", "")]
	public OperateResult Write(string address, int value)
	{
		return ReadRpc<string>(GetTopic("WriteInt32"), new { address, value });
	}

	[HslMqttApi("WriteUInt32Array", "")]
	public virtual OperateResult Write(string address, uint[] values)
	{
		return ReadRpc<string>(GetTopic("WriteUInt32Array"), new { address, values });
	}

	[HslMqttApi("WriteUInt32", "")]
	public OperateResult Write(string address, uint value)
	{
		return ReadRpc<string>(GetTopic("WriteUInt32"), new { address, value });
	}

	[HslMqttApi("WriteFloatArray", "")]
	public virtual OperateResult Write(string address, float[] values)
	{
		return ReadRpc<string>(GetTopic("WriteFloatArray"), new { address, values });
	}

	[HslMqttApi("WriteFloat", "")]
	public OperateResult Write(string address, float value)
	{
		return ReadRpc<string>(GetTopic("WriteFloat"), new { address, value });
	}

	[HslMqttApi("WriteInt64Array", "")]
	public virtual OperateResult Write(string address, long[] values)
	{
		return ReadRpc<string>(GetTopic("WriteInt64Array"), new { address, values });
	}

	[HslMqttApi("WriteInt64", "")]
	public OperateResult Write(string address, long value)
	{
		return ReadRpc<string>(GetTopic("WriteInt64"), new { address, value });
	}

	[HslMqttApi("WriteUInt64Array", "")]
	public virtual OperateResult Write(string address, ulong[] values)
	{
		return ReadRpc<string>(GetTopic("WriteUInt64Array"), new { address, values });
	}

	[HslMqttApi("WriteUInt64", "")]
	public OperateResult Write(string address, ulong value)
	{
		return ReadRpc<string>(GetTopic("WriteUInt64"), new { address, value });
	}

	[HslMqttApi("WriteDoubleArray", "")]
	public virtual OperateResult Write(string address, double[] values)
	{
		return ReadRpc<string>(GetTopic("WriteDoubleArray"), new { address, values });
	}

	[HslMqttApi("WriteDouble", "")]
	public OperateResult Write(string address, double value)
	{
		return ReadRpc<string>(GetTopic("WriteDouble"), new { address, value });
	}

	[HslMqttApi("WriteString", "")]
	public virtual OperateResult Write(string address, string value)
	{
		return ReadRpc<string>(GetTopic("WriteString"), new { address, value });
	}

	public virtual OperateResult Write(string address, string value, int length)
	{
		return Write(address, value, length, Encoding.ASCII);
	}

	public virtual OperateResult Write(string address, string value, Encoding encoding)
	{
		byte[] value2 = ByteTransform.TransByte(value, encoding);
		return Write(address, value2);
	}

	public virtual OperateResult Write(string address, string value, int length, Encoding encoding)
	{
		byte[] data = ByteTransform.TransByte(value, encoding);
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
		return await ReadRpcAsync<byte[]>(GetTopic("ReadByteArray"), new { address, length });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteByteArray"), new
		{
			address = address,
			value = value.ToHexString()
		});
	}

	public virtual async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		return await ReadRpcAsync<bool[]>(GetTopic("ReadBoolArray"), new { address, length });
	}

	public virtual async Task<OperateResult<bool>> ReadBoolAsync(string address)
	{
		return await ReadRpcAsync<bool>(GetTopic("ReadBool"), new { address });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteBoolArray"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, bool value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteBool"), new { address, value });
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
		return await ReadRpcAsync<short>(GetTopic("ReadInt16"), new { address });
	}

	public virtual async Task<OperateResult<short[]>> ReadInt16Async(string address, ushort length)
	{
		return await ReadRpcAsync<short[]>(GetTopic("ReadInt16Array"), new { address, length });
	}

	public async Task<OperateResult<ushort>> ReadUInt16Async(string address)
	{
		return await ReadRpcAsync<ushort>(GetTopic("ReadUInt16"), new { address });
	}

	public virtual async Task<OperateResult<ushort[]>> ReadUInt16Async(string address, ushort length)
	{
		return await ReadRpcAsync<ushort[]>(GetTopic("ReadUInt16Array"), new { address, length });
	}

	public async Task<OperateResult<int>> ReadInt32Async(string address)
	{
		return await ReadRpcAsync<int>(GetTopic("ReadInt32"), new { address });
	}

	public virtual async Task<OperateResult<int[]>> ReadInt32Async(string address, ushort length)
	{
		return await ReadRpcAsync<int[]>(GetTopic("ReadInt32Array"), new { address, length });
	}

	public async Task<OperateResult<uint>> ReadUInt32Async(string address)
	{
		return await ReadRpcAsync<uint>(GetTopic("ReadUInt32"), new { address });
	}

	public virtual async Task<OperateResult<uint[]>> ReadUInt32Async(string address, ushort length)
	{
		return await ReadRpcAsync<uint[]>(GetTopic("ReadUInt32Array"), new { address, length });
	}

	public async Task<OperateResult<float>> ReadFloatAsync(string address)
	{
		return await ReadRpcAsync<float>(GetTopic("ReadFloat"), new { address });
	}

	public virtual async Task<OperateResult<float[]>> ReadFloatAsync(string address, ushort length)
	{
		return await ReadRpcAsync<float[]>(GetTopic("ReadFloatArray"), new { address, length });
	}

	public async Task<OperateResult<long>> ReadInt64Async(string address)
	{
		return await ReadRpcAsync<long>(GetTopic("ReadInt64"), new { address });
	}

	public virtual async Task<OperateResult<long[]>> ReadInt64Async(string address, ushort length)
	{
		return await ReadRpcAsync<long[]>(GetTopic("ReadInt64Array"), new { address, length });
	}

	public async Task<OperateResult<ulong>> ReadUInt64Async(string address)
	{
		return await ReadRpcAsync<ulong>(GetTopic("ReadUInt64"), new { address });
	}

	public virtual async Task<OperateResult<ulong[]>> ReadUInt64Async(string address, ushort length)
	{
		return await ReadRpcAsync<ulong[]>(GetTopic("ReadUInt64Array"), new { address, length });
	}

	public async Task<OperateResult<double>> ReadDoubleAsync(string address)
	{
		return await ReadRpcAsync<double>(GetTopic("ReadDouble"), new { address });
	}

	public virtual async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		return await ReadRpcAsync<double[]>(GetTopic("ReadDoubleArray"), new { address, length });
	}

	public virtual async Task<OperateResult<string>> ReadStringAsync(string address, ushort length)
	{
		return await ReadRpcAsync<string>(GetTopic("ReadString"), new { address, length });
	}

	public virtual async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromBytes(await ReadAsync(address, length), (byte[] m) => ByteTransform.TransString(m, 0, m.Length, encoding));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, short[] values)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteInt16Array"), new { address, values });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, short value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteInt16"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, ushort[] values)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteUInt16Array"), new { address, values });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, ushort value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteUInt16"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, int[] values)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteInt32Array"), new { address, values });
	}

	public async Task<OperateResult> WriteAsync(string address, int value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteInt32"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, uint[] values)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteUInt32Array"), new { address, values });
	}

	public async Task<OperateResult> WriteAsync(string address, uint value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteUInt32"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, float[] values)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteFloatArray"), new { address, values });
	}

	public async Task<OperateResult> WriteAsync(string address, float value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteFloat"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, long[] values)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteInt64Array"), new { address, values });
	}

	public async Task<OperateResult> WriteAsync(string address, long value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteInt64"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, ulong[] values)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteUInt64Array"), new { address, values });
	}

	public async Task<OperateResult> WriteAsync(string address, ulong value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteUInt64"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteDoubleArray"), new { address, values });
	}

	public async Task<OperateResult> WriteAsync(string address, double value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteDouble"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value)
	{
		return await ReadRpcAsync<string>(GetTopic("WriteString"), new { address, value });
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value, Encoding encoding)
	{
		byte[] temp = ByteTransform.TransByte(value, encoding);
		return await WriteAsync(address, temp);
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value, int length)
	{
		return await WriteAsync(address, value, length, Encoding.ASCII);
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value, int length, Encoding encoding)
	{
		byte[] temp2 = ByteTransform.TransByte(value, encoding);
		temp2 = SoftBasic.ArrayExpandToLength(temp2, length);
		return await WriteAsync(address, temp2);
	}
}
