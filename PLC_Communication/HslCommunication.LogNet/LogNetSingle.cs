using System;
using System.IO;
using System.Text;

namespace HslCommunication.LogNet;

public class LogNetSingle : LogNetBase, ILogNet, IDisposable
{
	private readonly string fileName = string.Empty;

	public LogNetSingle(string filePath)
	{
		fileName = filePath;
		base.LogSaveMode = LogSaveMode.SingleFile;
		if (!string.IsNullOrEmpty(fileName))
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if (!Directory.Exists(fileInfo.DirectoryName))
			{
				Directory.CreateDirectory(fileInfo.DirectoryName);
			}
		}
	}

	public void ClearLog()
	{
		m_fileSaveLock.Enter();
		try
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				File.Create(fileName).Dispose();
			}
		}
		catch
		{
			throw;
		}
		finally
		{
			m_fileSaveLock.Leave();
		}
	}

	public string GetAllSavedLog()
	{
		string result = string.Empty;
		m_fileSaveLock.Enter();
		try
		{
			if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
			{
				StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8);
				result = streamReader.ReadToEnd();
				streamReader.Dispose();
			}
		}
		catch
		{
			throw;
		}
		finally
		{
			m_fileSaveLock.Leave();
		}
		return result;
	}

	public string[] GetExistLogFileNames()
	{
		return new string[1] { fileName };
	}

	protected override string GetFileSaveName()
	{
		return fileName;
	}

	public override string ToString()
	{
		return "LogNetSingle[" + fileName + "]";
	}
}
