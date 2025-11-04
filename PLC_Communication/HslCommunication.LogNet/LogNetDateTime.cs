using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace HslCommunication.LogNet;

public class LogNetDateTime : LogPathBase, ILogNet, IDisposable
{
	private GenerateMode generateMode = GenerateMode.ByEveryYear;

	private int fileSize = -1;

	public LogNetDateTime(string filePath, GenerateMode generateMode = GenerateMode.ByEveryYear, int fileQuantity = -1)
	{
		base.filePath = filePath;
		this.generateMode = generateMode;
		base.LogSaveMode = LogSaveMode.Time;
		controlFileQuantity = fileQuantity;
		if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath))
		{
			Directory.CreateDirectory(filePath);
		}
	}

	public LogNetDateTime(string filePath, GenerateMode generateMode, int timeQuantity, int fileSize)
		: this(filePath, generateMode, timeQuantity)
	{
		this.fileSize = fileSize;
	}

	private string GetFileSaveName(int index)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return string.Empty;
		}
		string text = ((index > 0) ? $"({index}).txt" : ".txt");
		switch (generateMode)
		{
		case GenerateMode.ByEveryMinute:
			return Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyyMMdd_HHmm") + text);
		case GenerateMode.ByEveryHour:
			return Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyyMMdd_HH") + text);
		case GenerateMode.ByEveryDay:
			return Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyyMMdd") + text);
		case GenerateMode.ByEveryWeek:
		{
			GregorianCalendar gregorianCalendar = new GregorianCalendar();
			int weekOfYear = gregorianCalendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
			return Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.Year + "_W" + weekOfYear + text);
		}
		case GenerateMode.ByEveryMonth:
			return Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyy_MM") + text);
		case GenerateMode.ByEverySeason:
			return Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.Year + "_Q" + (DateTime.Now.Month / 3 + 1) + text);
		case GenerateMode.ByEveryYear:
			return Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.Year + text);
		default:
			return string.Empty;
		}
	}

	protected override string GetFileSaveName()
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return string.Empty;
		}
		if (fileSize <= 0)
		{
			return GetFileSaveName(0);
		}
		int num = 0;
		string fileSaveName;
		while (true)
		{
			fileSaveName = GetFileSaveName(num);
			if (!File.Exists(fileSaveName))
			{
				break;
			}
			try
			{
				FileInfo fileInfo = new FileInfo(fileSaveName);
				if (fileInfo.Length < fileSize)
				{
					return fileSaveName;
				}
			}
			catch
			{
				return fileSaveName;
			}
			num++;
		}
		return fileSaveName;
	}

	private DateTime GetDeleteTime()
	{
		GenerateMode generateMode = this.generateMode;
		if (1 == 0)
		{
		}
		DateTime result = generateMode switch
		{
			GenerateMode.ByEveryMinute => DateTime.Now.AddMinutes(-controlFileQuantity), 
			GenerateMode.ByEveryHour => DateTime.Now.AddHours(-controlFileQuantity), 
			GenerateMode.ByEveryDay => DateTime.Now.AddDays(-controlFileQuantity), 
			GenerateMode.ByEveryWeek => DateTime.Now.AddDays(-controlFileQuantity * 7), 
			GenerateMode.ByEveryMonth => DateTime.Now.AddMonths(-controlFileQuantity), 
			GenerateMode.ByEverySeason => DateTime.Now.AddMonths(-controlFileQuantity * 3), 
			GenerateMode.ByEveryYear => DateTime.Now.AddYears(-controlFileQuantity), 
			_ => DateTime.Now.AddDays(-controlFileQuantity), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	protected override void OnWriteCompleted(bool createNewLogFile)
	{
		if (fileSize <= 0)
		{
			base.OnWriteCompleted(createNewLogFile);
		}
		else
		{
			if (!createNewLogFile || controlFileQuantity <= 1)
			{
				return;
			}
			try
			{
				string[] existLogFileNames = GetExistLogFileNames();
				if (existLogFileNames.Length > controlFileQuantity)
				{
					List<FileInfo> list = new List<FileInfo>();
					for (int i = 0; i < existLogFileNames.Length; i++)
					{
						list.Add(new FileInfo(existLogFileNames[i]));
					}
					list.Sort((FileInfo m, FileInfo n) => m.CreationTime.CompareTo(n.CreationTime));
					DateTime deleteTime = GetDeleteTime();
					for (int num = 0; num < list.Count && list[num].CreationTime < deleteTime; num++)
					{
						File.Delete(list[num].FullName);
					}
				}
			}
			catch
			{
			}
		}
	}

	public override string ToString()
	{
		return $"LogNetDateTime[{generateMode}]";
	}
}
