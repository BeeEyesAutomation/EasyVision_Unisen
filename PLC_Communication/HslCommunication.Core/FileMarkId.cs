using System;
using System.Collections.Generic;
using HslCommunication.LogNet;

namespace HslCommunication.Core;

public class FileMarkId
{
	private int readStatus = 0;

	private readonly ILogNet logNet;

	private readonly string fileName = null;

	private readonly Queue<Action> queues = new Queue<Action>();

	private readonly object hybirdLock = new object();

	public DateTime CreateTime { get; private set; }

	public DateTime ActiveTime { get; private set; }

	public long DownloadTimes { get; private set; }

	public FileMarkId(ILogNet logNet, string fileName)
	{
		this.logNet = logNet;
		this.fileName = fileName;
		CreateTime = DateTime.Now;
		ActiveTime = DateTime.Now;
	}

	public void AddOperation(Action action)
	{
		lock (hybirdLock)
		{
			if (readStatus == 0)
			{
				action?.Invoke();
				return;
			}
			logNet?.WriteDebug(ToString(), "action delay");
			queues.Enqueue(action);
		}
	}

	public bool CanClear()
	{
		bool result = false;
		lock (hybirdLock)
		{
			result = readStatus == 0 && queues.Count == 0;
		}
		return result;
	}

	public void EnterReadOperator()
	{
		lock (hybirdLock)
		{
			readStatus++;
			ActiveTime = DateTime.Now;
		}
	}

	public void LeaveReadOperator()
	{
		lock (hybirdLock)
		{
			readStatus--;
			DownloadTimes++;
			if (readStatus == 0)
			{
				while (queues.Count > 0)
				{
					try
					{
						queues.Dequeue()?.Invoke();
						logNet?.WriteDebug(ToString(), "action delay execute");
					}
					catch (Exception ex)
					{
						logNet?.WriteException("FileMarkId", "File Action Failed:", ex);
					}
				}
			}
			ActiveTime = DateTime.Now;
		}
	}

	public override string ToString()
	{
		return "FileMarkId[" + fileName + "]";
	}
}
