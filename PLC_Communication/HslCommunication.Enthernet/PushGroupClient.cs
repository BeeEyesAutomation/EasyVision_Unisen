using System;
using System.Collections.Generic;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class PushGroupClient : IDisposable
{
	private List<AppSession> appSessions;

	private SimpleHybirdLock simpleHybird;

	private long pushTimesCount = 0L;

	private bool disposedValue = false;

	public PushGroupClient()
	{
		appSessions = new List<AppSession>();
		simpleHybird = new SimpleHybirdLock();
	}

	public void AddPushClient(AppSession session)
	{
		simpleHybird.Enter();
		appSessions.Add(session);
		simpleHybird.Leave();
	}

	public bool RemovePushClient(string clientID)
	{
		bool result = false;
		simpleHybird.Enter();
		for (int i = 0; i < appSessions.Count; i++)
		{
			if (appSessions[i].ClientUniqueID == clientID)
			{
				appSessions[i].WorkSocket?.Close();
				appSessions.RemoveAt(i);
				result = true;
				break;
			}
		}
		simpleHybird.Leave();
		return result;
	}

	public void PushString(string content, Action<AppSession, string> send)
	{
		simpleHybird.Enter();
		Interlocked.Increment(ref pushTimesCount);
		for (int i = 0; i < appSessions.Count; i++)
		{
			send(appSessions[i], content);
		}
		simpleHybird.Leave();
	}

	public int RemoveAllClient()
	{
		int num = 0;
		simpleHybird.Enter();
		for (int i = 0; i < appSessions.Count; i++)
		{
			appSessions[i].WorkSocket?.Close();
		}
		num = appSessions.Count;
		appSessions.Clear();
		simpleHybird.Leave();
		return num;
	}

	public bool HasPushedContent()
	{
		return pushTimesCount > 0;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
			}
			simpleHybird.Enter();
			appSessions.ForEach(delegate(AppSession m)
			{
				m.WorkSocket?.Close();
			});
			appSessions.Clear();
			simpleHybird.Leave();
			simpleHybird.Dispose();
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public override string ToString()
	{
		return "PushGroupClient";
	}
}
