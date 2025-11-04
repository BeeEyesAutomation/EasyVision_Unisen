using System;
using System.IO;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.Reflection;

namespace HslCommunication.LogNet;

public class LogStatistics : LogStatisticsBase<long>
{
	private RegularByteTransform byteTransform;

	private long totalSum = 0L;

	[HslMqttApi(HttpMethod = "GET", Description = "Get the sum of all current values")]
	public long TotalSum => totalSum;

	public LogStatistics(GenerateMode generateMode, int dataCount)
		: base(generateMode, dataCount)
	{
		byteTransform = new RegularByteTransform();
	}

	[HslMqttApi(Description = "Adding a new statistical information will determine the position to insert the data according to the current time.")]
	public void StatisticsAdd(long frequency = 1L)
	{
		Interlocked.Add(ref totalSum, frequency);
		StatisticsCustomAction((long m) => m + frequency);
	}

	[HslMqttApi(Description = "Adding a new statistical information will determine the position to insert the data according to the specified time.")]
	public void StatisticsAddByTime(long frequency, DateTime time)
	{
		Interlocked.Add(ref totalSum, frequency);
		StatisticsCustomAction((long m) => m + frequency, time);
	}

	public byte[] SaveToBinary()
	{
		OperateResult<long, long[]> statisticsSnapAndDataMark = GetStatisticsSnapAndDataMark();
		int num = 1024;
		byte[] array = new byte[statisticsSnapAndDataMark.Content2.Length * 8 + num];
		BitConverter.GetBytes(305419896).CopyTo(array, 0);
		BitConverter.GetBytes((ushort)num).CopyTo(array, 4);
		BitConverter.GetBytes((ushort)base.GenerateMode).CopyTo(array, 6);
		BitConverter.GetBytes(statisticsSnapAndDataMark.Content2.Length).CopyTo(array, 8);
		BitConverter.GetBytes(statisticsSnapAndDataMark.Content1).CopyTo(array, 12);
		BitConverter.GetBytes(TotalSum).CopyTo(array, 20);
		for (int i = 0; i < statisticsSnapAndDataMark.Content2.Length; i++)
		{
			BitConverter.GetBytes(statisticsSnapAndDataMark.Content2[i]).CopyTo(array, num + i * 8);
		}
		return array;
	}

	public void SaveToFile(string fileName)
	{
		File.WriteAllBytes(fileName, SaveToBinary());
	}

	public void LoadFromBinary(byte[] buffer)
	{
		int num = BitConverter.ToInt32(buffer, 0);
		if (num != 305419896)
		{
			throw new Exception("File is not LogStatistics file, can't load data.");
		}
		int index = BitConverter.ToUInt16(buffer, 4);
		GenerateMode generateMode = (GenerateMode)BitConverter.ToUInt16(buffer, 6);
		int length = BitConverter.ToInt32(buffer, 8);
		long num2 = BitConverter.ToInt64(buffer, 12);
		long num3 = BitConverter.ToInt64(buffer, 20);
		base.generateMode = generateMode;
		totalSum = num3;
		long[] array = byteTransform.TransInt64(buffer, index, length);
		Reset(array, num2);
	}

	public void LoadFromFile(string fileName)
	{
		if (File.Exists(fileName))
		{
			LoadFromBinary(File.ReadAllBytes(fileName));
		}
	}

	public override string ToString()
	{
		return $"LogStatistics[{base.GenerateMode}:{base.ArrayLength}]";
	}
}
