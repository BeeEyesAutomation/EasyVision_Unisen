using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HslCommunication.Core;
using HslCommunication.Reflection;

namespace HslCommunication.LogNet;

public class LogStatisticsDict
{
	private GenerateMode generateMode = GenerateMode.ByEveryDay;

	private int arrayLength = 30;

	private Dictionary<string, LogStatistics> dict;

	private object dictLock;

	private LogStatistics logStat;

	public GenerateMode GenerateMode => generateMode;

	[HslMqttApi(HttpMethod = "GET", Description = "Get the total amount of current statistical information")]
	public int ArrayLength => arrayLength;

	[HslMqttApi(PropertyUnfold = true, Description = "Get the log statistics object of the current dictionary class itself, and count the statistics of all elements")]
	public LogStatistics LogStat => logStat;

	public LogStatisticsDict(GenerateMode generateMode, int arrayLength)
	{
		this.generateMode = generateMode;
		this.arrayLength = arrayLength;
		dictLock = new object();
		dict = new Dictionary<string, LogStatistics>(128);
		logStat = new LogStatistics(generateMode, arrayLength);
	}

	public LogStatistics GetLogStatistics(string key)
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

	public void AddLogStatistics(string key, LogStatistics logStatistics)
	{
		lock (dictLock)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = logStatistics;
			}
			else
			{
				dict.Add(key, logStatistics);
			}
		}
	}

	public bool RemoveLogStatistics(string key)
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

	[HslMqttApi(Description = "Adding a new statistical information will determine the position to insert the data according to the current time")]
	public void StatisticsAdd(string key, long frequency = 1L)
	{
		logStat.StatisticsAdd(frequency);
		LogStatistics logStatistics = GetLogStatistics(key);
		if (logStatistics == null)
		{
			lock (dictLock)
			{
				if (!dict.ContainsKey(key))
				{
					logStatistics = new LogStatistics(generateMode, arrayLength);
					dict.Add(key, logStatistics);
				}
				else
				{
					logStatistics = dict[key];
				}
			}
		}
		logStatistics?.StatisticsAdd(frequency);
	}

	[HslMqttApi(Description = "Adding a new statistical information will determine the position to insert the data according to the specified time")]
	public void StatisticsAddByTime(string key, long frequency, DateTime time)
	{
		logStat.StatisticsAddByTime(frequency, time);
		LogStatistics logStatistics = GetLogStatistics(key);
		if (logStatistics == null)
		{
			lock (dictLock)
			{
				if (!dict.ContainsKey(key))
				{
					logStatistics = new LogStatistics(generateMode, arrayLength);
					dict.Add(key, logStatistics);
				}
				else
				{
					logStatistics = dict[key];
				}
			}
		}
		logStatistics?.StatisticsAddByTime(frequency, time);
	}

	[HslMqttApi(Description = "Get a data snapshot of the current statistics")]
	public long[] GetStatisticsSnapshot(string key)
	{
		return GetLogStatistics(key)?.GetStatisticsSnapshot();
	}

	[HslMqttApi(Description = "Get a snapshot of statistical data information according to the specified time range")]
	public long[] GetStatisticsSnapshotByTime(string key, DateTime start, DateTime finish)
	{
		return GetLogStatistics(key)?.GetStatisticsSnapshotByTime(start, finish);
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
			LogStatistics logStatistics = GetLogStatistics(text);
			if (logStatistics != null)
			{
				HslHelper.WriteStringToStream(fileStream, text);
				HslHelper.WriteBinaryToStream(fileStream, logStatistics.SaveToBinary());
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
			LogStatistics logStatistics = new LogStatistics(generateMode, arrayLength);
			logStatistics.LoadFromBinary(buffer);
			AddLogStatistics(key, logStatistics);
		}
	}
}
