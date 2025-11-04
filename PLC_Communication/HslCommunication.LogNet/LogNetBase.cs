using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.Reflection;

namespace HslCommunication.LogNet;

public abstract class LogNetBase : IDisposable
{
	protected SimpleHybirdLock m_fileSaveLock;

	private HslMessageDegree m_messageDegree = HslMessageDegree.DEBUG;

	private Queue<HslMessageItem> m_WaitForSave;

	private SimpleHybirdLock m_simpleHybirdLock;

	private int m_SaveStatus = 0;

	private List<string> filtrateKeyword;

	private object filtrateLock;

	private string lastLogSaveFileName = string.Empty;

	private int lastConsoleDegree = -1;

	private bool disposedValue = false;

	public LogSaveMode LogSaveMode { get; protected set; }

	public LogStatistics LogNetStatistics { get; set; }

	public bool ConsoleOutput { get; set; }

	public bool LogThreadID { get; set; } = true;

	public bool LogStxAsciiCode { get; set; } = true;

	public int HourDeviation { get; set; } = 0;

	public bool EncoderShouldEmitUTF8Identifier { get; set; } = true;

	public bool ConsoleColorEnable { get; set; } = true;

	public event EventHandler<HslEventArgs> BeforeSaveToFile = null;

	public LogNetBase()
	{
		m_fileSaveLock = new SimpleHybirdLock();
		m_simpleHybirdLock = new SimpleHybirdLock();
		m_WaitForSave = new Queue<HslMessageItem>();
		filtrateKeyword = new List<string>();
		filtrateLock = new object();
	}

	private void OnBeforeSaveToFile(HslEventArgs args)
	{
		this.BeforeSaveToFile?.Invoke(this, args);
	}

	[HslMqttApi]
	public void WriteDebug(string text)
	{
		WriteDebug(string.Empty, text);
	}

	[HslMqttApi(ApiTopic = "WriteDebugKeyWord")]
	public void WriteDebug(string keyWord, string text)
	{
		RecordMessage(HslMessageDegree.DEBUG, keyWord, text);
	}

	[HslMqttApi]
	public void WriteInfo(string text)
	{
		WriteInfo(string.Empty, text);
	}

	[HslMqttApi(ApiTopic = "WriteInfoKeyWord")]
	public void WriteInfo(string keyWord, string text)
	{
		RecordMessage(HslMessageDegree.INFO, keyWord, text);
	}

	[HslMqttApi]
	public void WriteWarn(string text)
	{
		WriteWarn(string.Empty, text);
	}

	[HslMqttApi(ApiTopic = "WriteWarnKeyWord")]
	public void WriteWarn(string keyWord, string text)
	{
		RecordMessage(HslMessageDegree.WARN, keyWord, text);
	}

	[HslMqttApi]
	public void WriteError(string text)
	{
		WriteError(string.Empty, text);
	}

	[HslMqttApi(ApiTopic = "WriteErrorKeyWord")]
	public void WriteError(string keyWord, string text)
	{
		RecordMessage(HslMessageDegree.ERROR, keyWord, text);
	}

	[HslMqttApi]
	public void WriteFatal(string text)
	{
		WriteFatal(string.Empty, text);
	}

	[HslMqttApi(ApiTopic = "WriteFatalKeyWord")]
	public void WriteFatal(string keyWord, string text)
	{
		RecordMessage(HslMessageDegree.FATAL, keyWord, text);
	}

	public void WriteException(string keyWord, Exception ex)
	{
		WriteException(keyWord, string.Empty, ex);
	}

	public void WriteException(string keyWord, string text, Exception ex)
	{
		RecordMessage(HslMessageDegree.FATAL, keyWord, LogNetManagment.GetSaveStringFromException(text, ex));
	}

	public void RecordMessage(HslMessageDegree degree, string keyWord, string text)
	{
		WriteToFile(degree, keyWord, text);
	}

	[HslMqttApi]
	public void WriteDescrition(string description)
	{
		if (string.IsNullOrEmpty(description))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder("\u0002");
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append("\u0002/");
		int num = 118 - CalculateStringOccupyLength(description);
		if (num >= 8)
		{
			int num2 = (num - 8) / 2;
			AppendCharToStringBuilder(stringBuilder, '*', num2);
			stringBuilder.Append("   ");
			stringBuilder.Append(description);
			stringBuilder.Append("   ");
			if (num % 2 == 0)
			{
				AppendCharToStringBuilder(stringBuilder, '*', num2);
			}
			else
			{
				AppendCharToStringBuilder(stringBuilder, '*', num2 + 1);
			}
		}
		else if (num >= 2)
		{
			int num3 = (num - 2) / 2;
			AppendCharToStringBuilder(stringBuilder, '*', num3);
			stringBuilder.Append(description);
			if (num % 2 == 0)
			{
				AppendCharToStringBuilder(stringBuilder, '*', num3);
			}
			else
			{
				AppendCharToStringBuilder(stringBuilder, '*', num3 + 1);
			}
		}
		else
		{
			stringBuilder.Append(description);
		}
		stringBuilder.Append("/");
		stringBuilder.Append(Environment.NewLine);
		RecordMessage(HslMessageDegree.None, string.Empty, stringBuilder.ToString());
	}

	[HslMqttApi]
	public void WriteAnyString(string text)
	{
		RecordMessage(HslMessageDegree.None, string.Empty, text);
	}

	[HslMqttApi]
	public void WriteNewLine()
	{
		RecordMessage(HslMessageDegree.None, string.Empty, "\u0002" + Environment.NewLine);
	}

	public void SetMessageDegree(HslMessageDegree degree)
	{
		m_messageDegree = degree;
	}

	[HslMqttApi]
	public void FiltrateKeyword(string keyword)
	{
		lock (filtrateLock)
		{
			if (!filtrateKeyword.Contains(keyword))
			{
				filtrateKeyword.Add(keyword);
			}
		}
	}

	[HslMqttApi]
	public void RemoveFiltrate(string keyword)
	{
		lock (filtrateLock)
		{
			if (filtrateKeyword.Contains(keyword))
			{
				filtrateKeyword.Remove(keyword);
			}
		}
	}

	private void WriteToFile(HslMessageDegree degree, string keyword, string text)
	{
		if (degree <= m_messageDegree)
		{
			HslMessageItem hslMessageItem = GetHslMessageItem(degree, keyword, text);
			AddItemToCache(hslMessageItem, start: true);
		}
	}

	private void AddItemToCache(HslMessageItem item, bool start)
	{
		m_simpleHybirdLock.Enter();
		m_WaitForSave.Enqueue(item);
		m_simpleHybirdLock.Leave();
		if (start)
		{
			StartSaveFile();
		}
	}

	private void StartSaveFile()
	{
		if (Interlocked.CompareExchange(ref m_SaveStatus, 1, 0) == 0)
		{
			ThreadPool.QueueUserWorkItem(ThreadPoolSaveFile, null);
		}
	}

	private HslMessageItem GetAndRemoveLogItem()
	{
		HslMessageItem result = null;
		m_simpleHybirdLock.Enter();
		try
		{
			result = ((m_WaitForSave.Count > 0) ? m_WaitForSave.Dequeue() : null);
		}
		catch
		{
		}
		m_simpleHybirdLock.Leave();
		return result;
	}

	private List<HslMessageItem> GetAndRemoveLogItemArray()
	{
		List<HslMessageItem> list = new List<HslMessageItem>();
		m_simpleHybirdLock.Enter();
		try
		{
			while (m_WaitForSave.Count > 0)
			{
				list.Add(m_WaitForSave.Dequeue());
			}
		}
		catch
		{
		}
		m_simpleHybirdLock.Leave();
		return list;
	}

	private void SetConsoleWriteForeColor(HslMessageDegree degree)
	{
		if (!ConsoleColorEnable)
		{
			return;
		}
		if (lastConsoleDegree < 0)
		{
			lastConsoleDegree = (int)degree;
		}
		if (lastConsoleDegree != (int)degree)
		{
			lastConsoleDegree = (int)degree;
			switch (degree)
			{
			case HslMessageDegree.DEBUG:
				Console.ForegroundColor = ConsoleColor.DarkGray;
				break;
			case HslMessageDegree.INFO:
				Console.ForegroundColor = ConsoleColor.White;
				break;
			case HslMessageDegree.WARN:
				Console.ForegroundColor = ConsoleColor.Yellow;
				break;
			case HslMessageDegree.ERROR:
				Console.ForegroundColor = ConsoleColor.Red;
				break;
			case HslMessageDegree.FATAL:
				Console.ForegroundColor = ConsoleColor.DarkRed;
				break;
			default:
				Console.ForegroundColor = ConsoleColor.White;
				break;
			}
		}
	}

	private void ConsoleWriteLog(HslMessageItem log)
	{
		SetConsoleWriteForeColor(log.Degree);
		Console.WriteLine(HslMessageFormat(log, writeFile: false));
	}

	private void ThreadPoolSaveFile(object obj)
	{
		HslMessageItem andRemoveLogItem = GetAndRemoveLogItem();
		m_fileSaveLock.Enter();
		string fileSaveName = GetFileSaveName();
		bool createNewLogFile = false;
		if (!string.IsNullOrEmpty(fileSaveName))
		{
			if (fileSaveName != lastLogSaveFileName)
			{
				createNewLogFile = true;
				lastLogSaveFileName = fileSaveName;
			}
			StreamWriter streamWriter = null;
			try
			{
				streamWriter = new StreamWriter(fileSaveName, append: true, EncoderShouldEmitUTF8Identifier ? Encoding.UTF8 : new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
				while (andRemoveLogItem != null)
				{
					if (!andRemoveLogItem.HasLogOutput)
					{
						andRemoveLogItem.HasLogOutput = true;
						if (ConsoleOutput)
						{
							ConsoleWriteLog(andRemoveLogItem);
						}
						OnBeforeSaveToFile(new HslEventArgs
						{
							HslMessage = andRemoveLogItem
						});
						LogNetStatistics?.StatisticsAdd(1L);
					}
					bool flag = true;
					lock (filtrateLock)
					{
						flag = !filtrateKeyword.Contains(andRemoveLogItem.KeyWord);
					}
					if (andRemoveLogItem.Cancel)
					{
						flag = false;
					}
					if (flag)
					{
						streamWriter.Write(HslMessageFormat(andRemoveLogItem, writeFile: true));
						streamWriter.Write(Environment.NewLine);
						streamWriter.Flush();
					}
					andRemoveLogItem = GetAndRemoveLogItem();
				}
			}
			catch (Exception ex)
			{
				if (ex is DirectoryNotFoundException)
				{
					try
					{
						string directoryName = Path.GetDirectoryName(fileSaveName);
						if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
					}
					catch
					{
					}
				}
				Interlocked.Increment(ref andRemoveLogItem.WriteRetryTimes);
				AddItemToCache(andRemoveLogItem, start: false);
			}
			finally
			{
				streamWriter?.Dispose();
			}
		}
		else if (ConsoleOutput)
		{
			if (andRemoveLogItem != null)
			{
				ConsoleWriteLog(andRemoveLogItem);
				OnBeforeSaveToFile(new HslEventArgs
				{
					HslMessage = andRemoveLogItem
				});
			}
			List<HslMessageItem> andRemoveLogItemArray = GetAndRemoveLogItemArray();
			while (andRemoveLogItemArray.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				HslMessageDegree degree = andRemoveLogItemArray[0].Degree;
				for (int i = 0; i < andRemoveLogItemArray.Count; i++)
				{
					if (andRemoveLogItemArray[i].Degree != degree)
					{
						SetConsoleWriteForeColor(degree);
						Console.Write(stringBuilder.ToString());
						degree = andRemoveLogItemArray[i].Degree;
						stringBuilder = new StringBuilder();
					}
					stringBuilder.AppendLine(HslMessageFormat(andRemoveLogItemArray[i], writeFile: false));
					if (i >= andRemoveLogItemArray.Count - 1)
					{
						SetConsoleWriteForeColor(degree);
						Console.Write(stringBuilder.ToString());
					}
					OnBeforeSaveToFile(new HslEventArgs
					{
						HslMessage = andRemoveLogItemArray[i]
					});
				}
				andRemoveLogItemArray = GetAndRemoveLogItemArray();
			}
		}
		else
		{
			while (andRemoveLogItem != null)
			{
				if (ConsoleOutput)
				{
					ConsoleWriteLog(andRemoveLogItem);
				}
				OnBeforeSaveToFile(new HslEventArgs
				{
					HslMessage = andRemoveLogItem
				});
				andRemoveLogItem = GetAndRemoveLogItem();
			}
		}
		m_fileSaveLock.Leave();
		Interlocked.Exchange(ref m_SaveStatus, 0);
		OnWriteCompleted(createNewLogFile);
		if (m_WaitForSave.Count > 0)
		{
			HslHelper.ThreadSleep(0);
			StartSaveFile();
		}
	}

	protected virtual string HslMessageFormat(HslMessageItem hslMessage, bool writeFile)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (hslMessage.Degree != HslMessageDegree.None)
		{
			if (writeFile && LogStxAsciiCode)
			{
				stringBuilder.Append("\u0002");
			}
			stringBuilder.Append("[");
			stringBuilder.Append(LogNetManagment.GetDegreeDescription(hslMessage.Degree));
			stringBuilder.Append("] ");
			stringBuilder.Append(hslMessage.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));
			stringBuilder.Append(" ");
			if (LogThreadID)
			{
				stringBuilder.Append("Thread:[");
				stringBuilder.Append(hslMessage.ThreadId.ToString("D3"));
				stringBuilder.Append("] ");
			}
			if (hslMessage.WriteRetryTimes == 2)
			{
				stringBuilder.Append("[Retry] ");
			}
			else if (hslMessage.WriteRetryTimes > 2)
			{
				stringBuilder.Append($"[Retry:{hslMessage.WriteRetryTimes}] ");
			}
			if (!string.IsNullOrEmpty(hslMessage.KeyWord))
			{
				stringBuilder.Append(hslMessage.KeyWord);
				stringBuilder.Append(" : ");
			}
		}
		stringBuilder.Append(hslMessage.Text);
		return stringBuilder.ToString();
	}

	public override string ToString()
	{
		return $"LogNetBase[{LogSaveMode}]";
	}

	protected virtual string GetFileSaveName()
	{
		return string.Empty;
	}

	protected virtual void OnWriteCompleted(bool createNewLogFile)
	{
	}

	private HslMessageItem GetHslMessageItem(HslMessageDegree degree, string keyWord, string text)
	{
		if (HourDeviation == 0)
		{
			return new HslMessageItem
			{
				KeyWord = keyWord,
				Degree = degree,
				Text = text,
				ThreadId = Thread.CurrentThread.ManagedThreadId
			};
		}
		return new HslMessageItem
		{
			KeyWord = keyWord,
			Degree = degree,
			Text = text,
			ThreadId = Thread.CurrentThread.ManagedThreadId,
			Time = DateTime.Now.AddHours(HourDeviation)
		};
	}

	private int CalculateStringOccupyLength(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < str.Length; i++)
		{
			num = ((str[i] < '一' || str[i] > '龻') ? (num + 1) : (num + 2));
		}
		return num;
	}

	private void AppendCharToStringBuilder(StringBuilder sb, char c, int count)
	{
		for (int i = 0; i < count; i++)
		{
			sb.Append(c);
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				this.BeforeSaveToFile = null;
				m_simpleHybirdLock.Enter();
				m_WaitForSave.Clear();
				m_simpleHybirdLock.Leave();
				m_simpleHybirdLock.Dispose();
				m_fileSaveLock.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}
}
