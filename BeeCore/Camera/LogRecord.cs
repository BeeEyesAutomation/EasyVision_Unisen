using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BeeCore
{
	internal class LogRecord
	{
		private static StreamWriter streamWriter;

		private static StringBuilder StringBuf = new StringBuilder();

		public static int RecordLevel = 2;

		public static void SaveLogFile()
		{
			try
			{
				if (StringBuf.Length > 0)
				{
					string text = Application.StartupPath + "\\Log\\";
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					text = text + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
					if (streamWriter == null)
					{
						streamWriter = ((!File.Exists(text)) ? File.CreateText(text) : File.AppendText(text));
					}
					streamWriter.WriteLine(StringBuf.ToString());
					StringBuf.Clear();
				}
			}
			finally
			{
				if (streamWriter != null)
				{
					streamWriter.Flush();
					streamWriter.Dispose();
					streamWriter = null;
				}
			}
		}

		public static void WriteError(Exception ex)
		{
			try
			{
				if (RecordLevel >= 0)
				{
					StringBuf.AppendLine("***************************************************");
					StringBuf.AppendLine("[ERROR] " + DateTime.Now.ToString("MM-dd HH:mm:ss:fff") + " : ");
					if (ex.TargetSite.Name != null)
					{
						StringBuf.AppendLine("异常函数：\r\n" + ex.TargetSite.Name);
					}
					if (ex.Message != null)
					{
						StringBuf.AppendLine("异常信息：\r\n" + ex.Message);
					}
					if (ex.StackTrace != null && RecordLevel == 0)
					{
						StringBuf.AppendLine("堆栈列表：\r\n" + ex.StackTrace);
					}
					StringBuf.AppendLine("--------------------------------------------------");
					if (StringBuf.Length > 10240)
					{
						SaveLogFile();
					}
				}
			}
			catch
			{
			}
		}

		public static void WriteInfo(string str)
		{
			if (RecordLevel > 2)
			{
				StringBuf.Append("[INFO] " + DateTime.Now.ToString("MM-dd HH:mm:ss:fff") + " : ");
				StringBuf.AppendLine(str);
				if (StringBuf.Length > 10240)
				{
					SaveLogFile();
				}
			}
		}

		public static void WriteWern(string str)
		{
			if (RecordLevel > 1)
			{
				StringBuf.Append("[WERN] " + DateTime.Now.ToString("MM-dd HH:mm:ss:fff") + " : ");
				StringBuf.AppendLine(str);
				if (StringBuf.Length > 10240)
				{
					SaveLogFile();
				}
			}
		}
	}
}
