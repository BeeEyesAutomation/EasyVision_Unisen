using System;
using System.IO;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.Reflection;

namespace HslCommunication.LogNet;

public class LogValueLimit : LogStatisticsBase<ValueLimit>
{
	private RegularByteTransform byteTransform;

	private long valueCount = 0L;

	[HslMqttApi(HttpMethod = "GET", Description = "The total amount of data currently set")]
	public long ValueCount => valueCount;

	public LogValueLimit(GenerateMode generateMode, int dataCount)
		: base(generateMode, dataCount)
	{
		byteTransform = new RegularByteTransform();
	}

	[HslMqttApi(Description = "Add a new data for analysis")]
	public void AnalysisNewValue(double value)
	{
		Interlocked.Increment(ref valueCount);
		StatisticsCustomAction((ValueLimit m) => m.SetNewValue(value));
	}

	[HslMqttApi(Description = "Add a new data for analysis")]
	public void AnalysisNewValueByTime(double value, DateTime time)
	{
		Interlocked.Increment(ref valueCount);
		StatisticsCustomAction((ValueLimit m) => m.SetNewValue(value), time);
	}

	public byte[] SaveToBinary()
	{
		OperateResult<long, ValueLimit[]> statisticsSnapAndDataMark = GetStatisticsSnapAndDataMark();
		int num = 1024;
		int num2 = 64;
		byte[] array = new byte[statisticsSnapAndDataMark.Content2.Length * num2 + num];
		BitConverter.GetBytes(305419897).CopyTo(array, 0);
		BitConverter.GetBytes((ushort)num).CopyTo(array, 4);
		BitConverter.GetBytes((ushort)base.GenerateMode).CopyTo(array, 6);
		BitConverter.GetBytes(statisticsSnapAndDataMark.Content2.Length).CopyTo(array, 8);
		BitConverter.GetBytes(statisticsSnapAndDataMark.Content1).CopyTo(array, 12);
		BitConverter.GetBytes(valueCount).CopyTo(array, 20);
		BitConverter.GetBytes(num2).CopyTo(array, 28);
		for (int i = 0; i < statisticsSnapAndDataMark.Content2.Length; i++)
		{
			byteTransform.TransByte(statisticsSnapAndDataMark.Content2[i].StartValue).CopyTo(array, i * num2 + num);
			byteTransform.TransByte(statisticsSnapAndDataMark.Content2[i].Current).CopyTo(array, i * num2 + num + 8);
			byteTransform.TransByte(statisticsSnapAndDataMark.Content2[i].MaxValue).CopyTo(array, i * num2 + num + 16);
			byteTransform.TransByte(statisticsSnapAndDataMark.Content2[i].MinValue).CopyTo(array, i * num2 + num + 24);
			byteTransform.TransByte(statisticsSnapAndDataMark.Content2[i].Average).CopyTo(array, i * num2 + num + 32);
			byteTransform.TransByte(statisticsSnapAndDataMark.Content2[i].Count).CopyTo(array, i * num2 + num + 40);
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
		if (num != 305419897)
		{
			throw new Exception("File is not LogValueLimit file, can't load data.");
		}
		int num2 = BitConverter.ToUInt16(buffer, 4);
		GenerateMode generateMode = (GenerateMode)BitConverter.ToUInt16(buffer, 6);
		int num3 = BitConverter.ToInt32(buffer, 8);
		long num4 = BitConverter.ToInt64(buffer, 12);
		long num5 = BitConverter.ToInt64(buffer, 20);
		int num6 = BitConverter.ToInt32(buffer, 28);
		base.generateMode = generateMode;
		valueCount = num5;
		ValueLimit[] array = new ValueLimit[num3];
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StartValue = byteTransform.TransDouble(buffer, i * num6 + num2);
			array[i].Current = byteTransform.TransDouble(buffer, i * num6 + num2 + 8);
			array[i].MaxValue = byteTransform.TransDouble(buffer, i * num6 + num2 + 16);
			array[i].MinValue = byteTransform.TransDouble(buffer, i * num6 + num2 + 24);
			array[i].Average = byteTransform.TransDouble(buffer, i * num6 + num2 + 32);
			array[i].Count = byteTransform.TransInt32(buffer, i * num6 + num2 + 40);
		}
		Reset(array, num4);
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
		return $"LogValueLimit[{base.GenerateMode}:{base.ArrayLength}]";
	}
}
