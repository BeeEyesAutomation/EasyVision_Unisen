using System.Collections.Generic;
using System.IO;

namespace HslCommunication.LogNet;

public abstract class LogPathBase : LogNetBase
{
	protected string fileName = string.Empty;

	protected string filePath = string.Empty;

	protected int controlFileQuantity = -1;

	protected override void OnWriteCompleted(bool createNewLogFile)
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
				for (int num = 0; num < list.Count - controlFileQuantity; num++)
				{
					File.Delete(list[num].FullName);
				}
			}
		}
		catch
		{
		}
	}

	public string[] GetExistLogFileNames()
	{
		if (!string.IsNullOrEmpty(filePath))
		{
			return Directory.GetFiles(filePath, LogNetManagment.LogFileHeadString + "*.txt");
		}
		return new string[0];
	}

	public override string ToString()
	{
		return "LogPathBase";
	}
}
