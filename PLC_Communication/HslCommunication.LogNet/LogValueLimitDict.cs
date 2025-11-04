using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HslCommunication.Core;
using HslCommunication.Reflection;

namespace HslCommunication.LogNet;

public class LogValueLimitDict
{
	private GenerateMode generateMode = GenerateMode.ByEveryDay;

	private int arrayLength = 30;

	private Dictionary<string, LogValueLimit> dict;

	private object dictLock;

	private LogStatistics logStat;

	public GenerateMode GenerateMode => generateMode;

	[HslMqttApi(HttpMethod = "GET", Description = "Get the total amount of current statistical information")]
	public int ArrayLength => arrayLength;

	[HslMqttApi(PropertyUnfold = true, Description = "Get the log statistics object of the current dictionary class itself, and count the statistics of all elements")]
	public LogStatistics LogStat => logStat;

	public LogValueLimitDict(GenerateMode generateMode, int arrayLength)
	{
		this.generateMode = generateMode;
		this.arrayLength = arrayLength;
		dictLock = new object();
		dict = new Dictionary<string, LogValueLimit>(128);
		logStat = new LogStatistics(generateMode, arrayLength);
	}

	public LogValueLimit GetLogValueLimit(string key)
	{
		lock (dictLock)
		{
			if (dict.ContainsKey(key))
			{
				return dict[key];
			}
			return null;
		}
	}

	public void AddLogValueLimit(string key, LogValueLimit logValueLimit)
	{
		lock (dictLock)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = logValueLimit;
			}
			else
			{
				dict.Add(key, logValueLimit);
			}
		}
	}

	public bool RemoveLogValueLimit(string key)
	{
		lock (dictLock)
		{
			if (dict.ContainsKey(key))
			{
				dict.Remove(key);
				return true;
			}
			return false;
		}
	}

	[HslMqttApi(Description = "Add a new data for analysis")]
	public void AnalysisNewValue(string key, double value)
	{
		logStat.StatisticsAdd(1L);
		LogValueLimit logValueLimit = GetLogValueLimit(key);
		if (logValueLimit == null)
		{
			lock (dictLock)
			{
				if (!dict.ContainsKey(key))
				{
					logValueLimit = new LogValueLimit(generateMode, arrayLength);
					dict.Add(key, logValueLimit);
				}
				else
				{
					logValueLimit = dict[key];
				}
			}
		}
		logValueLimit?.AnalysisNewValue(value);
	}

	[HslMqttApi(Description = "Add a new data for analysis")]
	public void AnalysisNewValueByTime(string key, double value, DateTime time)
	{
		logStat.StatisticsAddByTime(1L, time);
		LogValueLimit logValueLimit = GetLogValueLimit(key);
		if (logValueLimit == null)
		{
			lock (dictLock)
			{
				if (!dict.ContainsKey(key))
				{
					logValueLimit = new LogValueLimit(generateMode, arrayLength);
					dict.Add(key, logValueLimit);
				}
				else
				{
					logValueLimit = dict[key];
				}
			}
		}
		logValueLimit?.AnalysisNewValueByTime(value, time);
	}

	[HslMqttApi(Description = "Get a data snapshot of the current statistics")]
	public ValueLimit[] GetStatisticsSnapshot(string key)
	{
		return GetLogValueLimit(key)?.GetStatisticsSnapshot();
	}

	[HslMqttApi(Description = "Get a snapshot of statistical data information according to the specified time range")]
	public ValueLimit[] GetStatisticsSnapshotByTime(string key, DateTime start, DateTime finish)
	{
		return GetLogValueLimit(key)?.GetStatisticsSnapshotByTime(start, finish);
	}

	[HslMqttApi(Description = "Get data information of all keywords")]
	public string[] GetKeys()
	{
		lock (dictLock)
		{
			return dict.Keys.ToArray();
		}
	}

	public void SaveToFile(string fileName)
	{
		using FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
		byte[] array = new byte[1024];
		BitConverter.GetBytes(305419906).CopyTo(array, 0);
		BitConverter.GetBytes((ushort)array.Length).CopyTo(array, 4);
		BitConverter.GetBytes((ushort)GenerateMode).CopyTo(array, 6);
		string[] keys = GetKeys();
		BitConverter.GetBytes(keys.Length).CopyTo(array, 8);
		fileStream.Write(array, 0, array.Length);
		string[] array2 = keys;
		string[] array3 = array2;
		foreach (string text in array3)
		{
			LogValueLimit logValueLimit = GetLogValueLimit(text);
			if (logValueLimit != null)
			{
				HslHelper.WriteStringToStream(fileStream, text);
				HslHelper.WriteBinaryToStream(fileStream, logValueLimit.SaveToBinary());
			}
		}
	}

	public void LoadFromFile(string fileName)
	{
		if (!File.Exists(fileName))
		{
			return;
		}
		using FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		byte[] value = HslHelper.ReadSpecifiedLengthFromStream(stream, 1024);
		generateMode = (GenerateMode)BitConverter.ToUInt16(value, 6);
		int num = BitConverter.ToInt32(value, 8);
		for (int i = 0; i < num; i++)
		{
			string key = HslHelper.ReadStringFromStream(stream);
			byte[] buffer = HslHelper.ReadBinaryFromStream(stream);
			LogValueLimit logValueLimit = new LogValueLimit(generateMode, arrayLength);
			logValueLimit.LoadFromBinary(buffer);
			AddLogValueLimit(key, logValueLimit);
		}
	}
}
