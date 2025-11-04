using System;
using System.IO;

namespace HslCommunication.LogNet;

public class LogNetFileSize : LogPathBase, ILogNet, IDisposable
{
	private int fileMaxSize = 2097152;

	private int currentFileSize = 0;

	public LogNetFileSize(string filePath, int fileMaxSize = 2097152, int fileQuantity = -1)
	{
		base.filePath = filePath;
		this.fileMaxSize = fileMaxSize;
		controlFileQuantity = fileQuantity;
		base.LogSaveMode = LogSaveMode.FileFixedSize;
		if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath))
		{
			Directory.CreateDirectory(filePath);
		}
	}

	protected override string GetFileSaveName()
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return string.Empty;
		}
		if (string.IsNullOrEmpty(fileName))
		{
			fileName = GetLastAccessFileName();
		}
		if (File.Exists(fileName))
		{
			FileInfo fileInfo = new FileInfo(fileName);
			if (fileInfo.Length > fileMaxSize)
			{
				fileName = GetDefaultFileName();
			}
			else
			{
				currentFileSize = (int)fileInfo.Length;
			}
		}
		return fileName;
	}

	private string GetLastAccessFileName()
	{
		string[] existLogFileNames = GetExistLogFileNames();
		string[] array = existLogFileNames;
		foreach (string result in array)
		{
			FileInfo fileInfo = new FileInfo(result);
			if (fileInfo.Length < fileMaxSize)
			{
				currentFileSize = (int)fileInfo.Length;
				return result;
			}
		}
		return GetDefaultFileName();
	}

	private string GetDefaultFileName()
	{
		return Path.Combine(filePath, LogNetManagment.LogFileHeadString + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
	}

	public override string ToString()
	{
		return $"LogNetFileSize[{fileMaxSize}]";
	}
}
