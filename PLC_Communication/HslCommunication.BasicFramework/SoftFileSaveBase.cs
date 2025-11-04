using System;
using System.IO;
using System.Text;
using HslCommunication.Core;
using HslCommunication.LogNet;

namespace HslCommunication.BasicFramework;

public class SoftFileSaveBase : ISoftFileSaveBase
{
	private SimpleHybirdLock HybirdLock;

	protected string LogHeaderText { get; set; }

	public string FileSavePath { get; set; }

	public ILogNet ILogNet { get; set; }

	public SoftFileSaveBase()
	{
		HybirdLock = new SimpleHybirdLock();
	}

	public virtual string ToSaveString()
	{
		return string.Empty;
	}

	public virtual void LoadByString(string content)
	{
	}

	public virtual void LoadByFile()
	{
		LoadByFile((string m) => m);
	}

	public void LoadByFile(Converter<string, string> decrypt)
	{
		if (!(FileSavePath != "") || !File.Exists(FileSavePath))
		{
			return;
		}
		HybirdLock.Enter();
		try
		{
			using StreamReader streamReader = new StreamReader(FileSavePath, Encoding.Default);
			LoadByString(decrypt(streamReader.ReadToEnd()));
		}
		catch (Exception ex)
		{
			ILogNet?.WriteException(LogHeaderText, StringResources.Language.FileLoadFailed, ex);
		}
		finally
		{
			HybirdLock.Leave();
		}
	}

	public virtual void SaveToFile()
	{
		SaveToFile((string m) => m);
	}

	public void SaveToFile(Converter<string, string> encrypt)
	{
		if (!(FileSavePath != ""))
		{
			return;
		}
		HybirdLock.Enter();
		try
		{
			using StreamWriter streamWriter = new StreamWriter(FileSavePath, append: false, Encoding.Default);
			streamWriter.Write(encrypt(ToSaveString()));
			streamWriter.Flush();
		}
		catch (Exception ex)
		{
			ILogNet?.WriteException(LogHeaderText, StringResources.Language.FileSaveFailed, ex);
		}
		finally
		{
			HybirdLock.Leave();
		}
	}
}
