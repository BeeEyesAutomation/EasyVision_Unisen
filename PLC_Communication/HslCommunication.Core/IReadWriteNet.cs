using System;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.LogNet;

namespace HslCommunication.Core;

public interface IReadWriteNet
{
	ILogNet LogNet { get; set; }

	string ConnectionId { get; set; }

	OperateResult<byte[]> Read(string address, ushort length);

	OperateResult Write(string address, byte[] value);

	OperateResult<bool[]> ReadBool(string address, ushort length);

	OperateResult<bool> ReadBool(string address);

	OperateResult Write(string address, bool[] value);

	OperateResult Write(string address, bool value);

	OperateResult<short> ReadInt16(string address);

	OperateResult<short[]> ReadInt16(string address, ushort length);

	OperateResult<ushort> ReadUInt16(string address);

	OperateResult<ushort[]> ReadUInt16(string address, ushort length);

	OperateResult<int> ReadInt32(string address);

	OperateResult<int[]> ReadInt32(string address, ushort length);

	OperateResult<uint> ReadUInt32(string address);

	OperateResult<uint[]> ReadUInt32(string address, ushort length);

	OperateResult<long> ReadInt64(string address);

	OperateResult<long[]> ReadInt64(string address, ushort length);

	OperateResult<ulong> ReadUInt64(string address);

	OperateResult<ulong[]> ReadUInt64(string address, ushort length);

	OperateResult<float> ReadFloat(string address);

	OperateResult<float[]> ReadFloat(string address, ushort length);

	OperateResult<double> ReadDouble(string address);

	OperateResult<double[]> ReadDouble(string address, ushort length);

	OperateResult<string> ReadString(string address, ushort length);

	OperateResult<string> ReadString(string address, ushort length, Encoding encoding);

	OperateResult<TimeSpan> Wait(string address, bool waitValue, int readInterval, int waitTimeout);

	OperateResult<TimeSpan> Wait(string address, short waitValue, int readInterval, int waitTimeout);

	OperateResult<TimeSpan> Wait(string address, ushort waitValue, int readInterval, int waitTimeout);

	OperateResult<TimeSpan> Wait(string address, int waitValue, int readInterval, int waitTimeout);

	OperateResult<TimeSpan> Wait(string address, uint waitValue, int readInterval, int waitTimeout);

	OperateResult<TimeSpan> Wait(string address, long waitValue, int readInterval, int waitTimeout);

	OperateResult<TimeSpan> Wait(string address, ulong waitValue, int readInterval, int waitTimeout);

	Task<OperateResult<TimeSpan>> WaitAsync(string address, bool waitValue, int readInterval, int waitTimeout);

	Task<OperateResult<TimeSpan>> WaitAsync(string address, short waitValue, int readInterval, int waitTimeout);

	Task<OperateResult<TimeSpan>> WaitAsync(string address, ushort waitValue, int readInterval, int waitTimeout);

	Task<OperateResult<TimeSpan>> WaitAsync(string address, int waitValue, int readInterval, int waitTimeout);

	Task<OperateResult<TimeSpan>> WaitAsync(string address, uint waitValue, int readInterval, int waitTimeout);

	Task<OperateResult<TimeSpan>> WaitAsync(string address, long waitValue, int readInterval, int waitTimeout);

	Task<OperateResult<TimeSpan>> WaitAsync(string address, ulong waitValue, int readInterval, int waitTimeout);

	OperateResult Write(string address, short value);

	OperateResult Write(string address, short[] values);

	OperateResult Write(string address, ushort value);

	OperateResult Write(string address, ushort[] values);

	OperateResult Write(string address, int value);

	OperateResult Write(string address, int[] values);

	OperateResult Write(string address, uint value);

	OperateResult Write(string address, uint[] values);

	OperateResult Write(string address, long value);

	OperateResult Write(string address, long[] values);

	OperateResult Write(string address, ulong value);

	OperateResult Write(string address, ulong[] values);

	OperateResult Write(string address, float value);

	OperateResult Write(string address, float[] values);

	OperateResult Write(string address, double value);

	OperateResult Write(string address, double[] values);

	OperateResult Write(string address, string value);

	OperateResult Write(string address, string value, Encoding encoding);

	OperateResult Write(string address, string value, int length);

	OperateResult Write(string address, string value, int length, Encoding encoding);

	OperateResult<T> ReadCustomer<T>(string address) where T : IDataTransfer, new();

	OperateResult<T> ReadCustomer<T>(string address, T obj) where T : IDataTransfer, new();

	OperateResult WriteCustomer<T>(string address, T value) where T : IDataTransfer, new();

	OperateResult<T> Read<T>() where T : class, new();

	OperateResult Write<T>(T data) where T : class, new();

	OperateResult<T> ReadStruct<T>(string address, ushort length) where T : class, new();

	Task<OperateResult<byte[]>> ReadAsync(string address, ushort length);

	Task<OperateResult> WriteAsync(string address, byte[] value);

	Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length);

	Task<OperateResult<bool>> ReadBoolAsync(string address);

	Task<OperateResult> WriteAsync(string address, bool[] value);

	Task<OperateResult> WriteAsync(string address, bool value);

	Task<OperateResult<short>> ReadInt16Async(string address);

	Task<OperateResult<short[]>> ReadInt16Async(string address, ushort length);

	Task<OperateResult<ushort>> ReadUInt16Async(string address);

	Task<OperateResult<ushort[]>> ReadUInt16Async(string address, ushort length);

	Task<OperateResult<int>> ReadInt32Async(string address);

	Task<OperateResult<int[]>> ReadInt32Async(string address, ushort length);

	Task<OperateResult<uint>> ReadUInt32Async(string address);

	Task<OperateResult<uint[]>> ReadUInt32Async(string address, ushort length);

	Task<OperateResult<long>> ReadInt64Async(string address);

	Task<OperateResult<long[]>> ReadInt64Async(string address, ushort length);

	Task<OperateResult<ulong>> ReadUInt64Async(string address);

	Task<OperateResult<ulong[]>> ReadUInt64Async(string address, ushort length);

	Task<OperateResult<float>> ReadFloatAsync(string address);

	Task<OperateResult<float[]>> ReadFloatAsync(string address, ushort length);

	Task<OperateResult<double>> ReadDoubleAsync(string address);

	Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length);

	Task<OperateResult<string>> ReadStringAsync(string address, ushort length);

	Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding);

	Task<OperateResult> WriteAsync(string address, short value);

	Task<OperateResult> WriteAsync(string address, short[] values);

	Task<OperateResult> WriteAsync(string address, ushort value);

	Task<OperateResult> WriteAsync(string address, ushort[] values);

	Task<OperateResult> WriteAsync(string address, int value);

	Task<OperateResult> WriteAsync(string address, int[] values);

	Task<OperateResult> WriteAsync(string address, uint value);

	Task<OperateResult> WriteAsync(string address, uint[] values);

	Task<OperateResult> WriteAsync(string address, long value);

	Task<OperateResult> WriteAsync(string address, long[] values);

	Task<OperateResult> WriteAsync(string address, ulong value);

	Task<OperateResult> WriteAsync(string address, ulong[] values);

	Task<OperateResult> WriteAsync(string address, float value);

	Task<OperateResult> WriteAsync(string address, float[] values);

	Task<OperateResult> WriteAsync(string address, double value);

	Task<OperateResult> WriteAsync(string address, double[] values);

	Task<OperateResult> WriteAsync(string address, string value);

	Task<OperateResult> WriteAsync(string address, string value, Encoding encoding);

	Task<OperateResult> WriteAsync(string address, string value, int length);

	Task<OperateResult> WriteAsync(string address, string value, int length, Encoding encoding);

	Task<OperateResult<T>> ReadCustomerAsync<T>(string address) where T : IDataTransfer, new();

	Task<OperateResult<T>> ReadCustomerAsync<T>(string address, T obj) where T : IDataTransfer, new();

	Task<OperateResult> WriteCustomerAsync<T>(string address, T value) where T : IDataTransfer, new();

	Task<OperateResult<T>> ReadAsync<T>() where T : class, new();

	Task<OperateResult> WriteAsync<T>(T data) where T : class, new();

	Task<OperateResult<T>> ReadStructAsync<T>(string address, ushort length) where T : class, new();
}
