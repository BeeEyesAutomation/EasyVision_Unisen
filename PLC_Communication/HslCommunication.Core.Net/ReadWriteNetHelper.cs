using System;
using System.Threading.Tasks;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Net;

public class ReadWriteNetHelper
{
	public static OperateResult WriteBoolWithWord(IReadWriteNet readWrite, string address, bool[] values, int addLength = 16, bool reverseWord = false, string bitStr = null)
	{
		string[] array = address.SplitDot();
		int num = 0;
		try
		{
			if (string.IsNullOrEmpty(bitStr))
			{
				if (array.Length > 1)
				{
					num = Convert.ToInt32(array[1]);
				}
			}
			else
			{
				num = Convert.ToInt32(bitStr);
			}
		}
		catch (Exception ex)
		{
			return new OperateResult(address + " Bit index input wrong: " + ex.Message);
		}
		ushort length = (ushort)((num + values.Length + addLength - 1) / addLength);
		OperateResult<byte[]> operateResult = readWrite.Read(array[0], length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		bool[] array2 = (reverseWord ? operateResult.Content.ReverseByWord().ToBoolArray() : operateResult.Content.ToBoolArray());
		if (num + values.Length <= array2.Length)
		{
			values.CopyTo(array2, num);
		}
		return readWrite.Write(array[0], reverseWord ? array2.ToByteArray().ReverseByWord() : array2.ToByteArray());
	}

	public static async Task<OperateResult> WriteBoolWithWordAsync(IReadWriteNet readWrite, string address, bool[] values, int addLength = 16, bool reverseWord = false, string bitStr = null)
	{
		string[] adds = address.SplitDot();
		int bit = 0;
		try
		{
			if (string.IsNullOrEmpty(bitStr))
			{
				if (adds.Length > 1)
				{
					bit = Convert.ToInt32(adds[1]);
				}
			}
			else
			{
				bit = Convert.ToInt32(bitStr);
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			return new OperateResult(address + " Bit index input wrong: " + ex3.Message);
		}
		OperateResult<byte[]> read = await readWrite.ReadAsync(length: (ushort)((bit + values.Length + addLength - 1) / addLength), address: adds[0]);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		bool[] array = (reverseWord ? read.Content.ReverseByWord().ToBoolArray() : read.Content.ToBoolArray());
		if (bit + values.Length <= array.Length)
		{
			values.CopyTo(array, bit);
		}
		return await readWrite.WriteAsync(adds[0], reverseWord ? array.ToByteArray().ReverseByWord() : array.ToByteArray());
	}

	public static OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, bool waitValue, int readInterval, int waitTimeout)
	{
		DateTime now = DateTime.Now;
		while (true)
		{
			OperateResult<bool> operateResult = readWriteNet.ReadBool(address);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(operateResult);
			}
			if (operateResult.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - now);
			}
			if (waitTimeout > 0 && (DateTime.Now - now).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			HslHelper.ThreadSleep(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, short waitValue, int readInterval, int waitTimeout)
	{
		DateTime now = DateTime.Now;
		while (true)
		{
			OperateResult<short> operateResult = readWriteNet.ReadInt16(address);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(operateResult);
			}
			if (operateResult.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - now);
			}
			if (waitTimeout > 0 && (DateTime.Now - now).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			HslHelper.ThreadSleep(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, ushort waitValue, int readInterval, int waitTimeout)
	{
		DateTime now = DateTime.Now;
		while (true)
		{
			OperateResult<ushort> operateResult = readWriteNet.ReadUInt16(address);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(operateResult);
			}
			if (operateResult.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - now);
			}
			if (waitTimeout > 0 && (DateTime.Now - now).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			HslHelper.ThreadSleep(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, int waitValue, int readInterval, int waitTimeout)
	{
		DateTime now = DateTime.Now;
		while (true)
		{
			OperateResult<int> operateResult = readWriteNet.ReadInt32(address);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(operateResult);
			}
			if (operateResult.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - now);
			}
			if (waitTimeout > 0 && (DateTime.Now - now).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			HslHelper.ThreadSleep(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, uint waitValue, int readInterval, int waitTimeout)
	{
		DateTime now = DateTime.Now;
		while (true)
		{
			OperateResult<uint> operateResult = readWriteNet.ReadUInt32(address);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(operateResult);
			}
			if (operateResult.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - now);
			}
			if (waitTimeout > 0 && (DateTime.Now - now).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			HslHelper.ThreadSleep(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, long waitValue, int readInterval, int waitTimeout)
	{
		DateTime now = DateTime.Now;
		while (true)
		{
			OperateResult<long> operateResult = readWriteNet.ReadInt64(address);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(operateResult);
			}
			if (operateResult.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - now);
			}
			if (waitTimeout > 0 && (DateTime.Now - now).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			HslHelper.ThreadSleep(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static OperateResult<TimeSpan> Wait(IReadWriteNet readWriteNet, string address, ulong waitValue, int readInterval, int waitTimeout)
	{
		DateTime now = DateTime.Now;
		while (true)
		{
			OperateResult<ulong> operateResult = readWriteNet.ReadUInt64(address);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(operateResult);
			}
			if (operateResult.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - now);
			}
			if (waitTimeout > 0 && (DateTime.Now - now).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			HslHelper.ThreadSleep(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static OperateResult<T> ReadCustomer<T>(IReadWriteNet readWriteNet, string address) where T : IDataTransfer, new()
	{
		T obj = new T();
		return ReadCustomer(readWriteNet, address, obj);
	}

	public static OperateResult<T> ReadCustomer<T>(IReadWriteNet readWriteNet, string address, T obj) where T : IDataTransfer, new()
	{
		OperateResult<byte[]> operateResult = readWriteNet.Read(address, obj.ReadCount);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(operateResult);
		}
		obj.ParseSource(operateResult.Content);
		return OperateResult.CreateSuccessResult(obj);
	}

	public static OperateResult WriteCustomer<T>(IReadWriteNet readWriteNet, string address, T data) where T : IDataTransfer, new()
	{
		return readWriteNet.Write(address, data.ToSource());
	}

	public static async Task<OperateResult<T>> ReadCustomerAsync<T>(IReadWriteNet readWriteNet, string address) where T : IDataTransfer, new()
	{
		T Content = new T();
		return await ReadCustomerAsync(readWriteNet, address, Content);
	}

	public static async Task<OperateResult<T>> ReadCustomerAsync<T>(IReadWriteNet readWriteNet, string address, T obj) where T : IDataTransfer, new()
	{
		OperateResult<byte[]> read = await readWriteNet.ReadAsync(address, obj.ReadCount);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(read);
		}
		obj.ParseSource(read.Content);
		return OperateResult.CreateSuccessResult(obj);
	}

	public static async Task<OperateResult> WriteCustomerAsync<T>(IReadWriteNet readWriteNet, string address, T data) where T : IDataTransfer, new()
	{
		return await readWriteNet.WriteAsync(address, data.ToSource());
	}

	public static OperateResult<T> ReadStruct<T>(IReadWriteNet readWriteNet, string address, ushort length, IByteTransform byteTransform, int startIndex = 0) where T : class, new()
	{
		OperateResult<byte[]> operateResult = readWriteNet.Read(address, length);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(operateResult);
		}
		try
		{
			return OperateResult.CreateSuccessResult(HslReflectionHelper.PraseStructContent<T>(operateResult.Content, startIndex, byteTransform));
		}
		catch (Exception ex)
		{
			return new OperateResult<T>("Prase struct faild: " + ex.Message + Environment.NewLine + "Source Data: " + operateResult.Content.ToHexString(' '));
		}
	}

	public static async Task<OperateResult<T>> ReadStructAsync<T>(IReadWriteNet readWriteNet, string address, ushort length, IByteTransform byteTransform, int startIndex = 0) where T : class, new()
	{
		OperateResult<byte[]> read = await readWriteNet.ReadAsync(address, length);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(read);
		}
		try
		{
			return OperateResult.CreateSuccessResult(HslReflectionHelper.PraseStructContent<T>(read.Content, startIndex, byteTransform));
		}
		catch (Exception ex)
		{
			return new OperateResult<T>("Prase struct faild: " + ex.Message + Environment.NewLine + "Source Data: " + read.Content.ToHexString(' '));
		}
	}

	public static async Task<OperateResult<TimeSpan>> WaitAsync(IReadWriteNet readWriteNet, string address, bool waitValue, int readInterval, int waitTimeout)
	{
		DateTime start = DateTime.Now;
		while (true)
		{
			OperateResult<bool> read = await readWriteNet.ReadBoolAsync(address);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(read);
			}
			if (read.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - start);
			}
			if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			await Task.Delay(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static async Task<OperateResult<TimeSpan>> WaitAsync(IReadWriteNet readWriteNet, string address, short waitValue, int readInterval, int waitTimeout)
	{
		DateTime start = DateTime.Now;
		while (true)
		{
			OperateResult<short> read = await readWriteNet.ReadInt16Async(address);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(read);
			}
			if (read.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - start);
			}
			if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			await Task.Delay(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static async Task<OperateResult<TimeSpan>> WaitAsync(IReadWriteNet readWriteNet, string address, ushort waitValue, int readInterval, int waitTimeout)
	{
		DateTime start = DateTime.Now;
		while (true)
		{
			OperateResult<ushort> read = await readWriteNet.ReadUInt16Async(address);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(read);
			}
			if (read.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - start);
			}
			if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			await Task.Delay(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static async Task<OperateResult<TimeSpan>> WaitAsync(IReadWriteNet readWriteNet, string address, int waitValue, int readInterval, int waitTimeout)
	{
		DateTime start = DateTime.Now;
		while (true)
		{
			OperateResult<int> read = await readWriteNet.ReadInt32Async(address);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(read);
			}
			if (read.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - start);
			}
			if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			await Task.Delay(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static async Task<OperateResult<TimeSpan>> WaitAsync(IReadWriteNet readWriteNet, string address, uint waitValue, int readInterval, int waitTimeout)
	{
		DateTime start = DateTime.Now;
		while (true)
		{
			OperateResult<uint> read = readWriteNet.ReadUInt32(address);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(read);
			}
			if (read.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - start);
			}
			if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			await Task.Delay(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static async Task<OperateResult<TimeSpan>> WaitAsync(IReadWriteNet readWriteNet, string address, long waitValue, int readInterval, int waitTimeout)
	{
		DateTime start = DateTime.Now;
		while (true)
		{
			OperateResult<long> read = readWriteNet.ReadInt64(address);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(read);
			}
			if (read.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - start);
			}
			if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			await Task.Delay(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}

	public static async Task<OperateResult<TimeSpan>> WaitAsync(IReadWriteNet readWriteNet, string address, ulong waitValue, int readInterval, int waitTimeout)
	{
		DateTime start = DateTime.Now;
		while (true)
		{
			OperateResult<ulong> read = readWriteNet.ReadUInt64(address);
			if (!read.IsSuccess)
			{
				return OperateResult.CreateFailedResult<TimeSpan>(read);
			}
			if (read.Content == waitValue)
			{
				return OperateResult.CreateSuccessResult(DateTime.Now - start);
			}
			if (waitTimeout > 0 && (DateTime.Now - start).TotalMilliseconds > (double)waitTimeout)
			{
				break;
			}
			await Task.Delay(readInterval);
		}
		return new OperateResult<TimeSpan>(StringResources.Language.CheckDataTimeout + waitTimeout);
	}
}
