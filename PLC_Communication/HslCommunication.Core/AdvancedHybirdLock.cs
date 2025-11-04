using System;
using System.Threading;

namespace HslCommunication.Core;

internal sealed class AdvancedHybirdLock : IDisposable
{
	private bool disposedValue = false;

	private int m_waiters = 0;

	private readonly Lazy<AutoResetEvent> m_waiterLock = new Lazy<AutoResetEvent>(() => new AutoResetEvent(initialState: false));

	private int m_spincount = 1000;

	private int m_owningThreadId = 0;

	private int m_recursion = 0;

	public int SpinCount
	{
		get
		{
			return m_spincount;
		}
		set
		{
			m_spincount = value;
		}
	}

	private void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
			}
			m_waiterLock.Value.Close();
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Enter()
	{
		int managedThreadId = Thread.CurrentThread.ManagedThreadId;
		if (managedThreadId == m_owningThreadId)
		{
			m_recursion++;
			return;
		}
		SpinWait spinWait = default(SpinWait);
		for (int i = 0; i < m_spincount; i++)
		{
			if (Interlocked.CompareExchange(ref m_waiters, 1, 0) == 0)
			{
				m_owningThreadId = Thread.CurrentThread.ManagedThreadId;
				m_recursion = 1;
				return;
			}
			spinWait.SpinOnce();
		}
		if (Interlocked.Increment(ref m_waiters) > 1)
		{
			m_waiterLock.Value.WaitOne();
		}
		m_owningThreadId = Thread.CurrentThread.ManagedThreadId;
		m_recursion = 1;
	}

	public void Leave()
	{
		if (Thread.CurrentThread.ManagedThreadId != m_owningThreadId)
		{
			throw new SynchronizationLockException("Current Thread have not the owning thread.");
		}
		if (--m_recursion <= 0)
		{
			m_owningThreadId = 0;
			if (Interlocked.Decrement(ref m_waiters) != 0)
			{
				m_waiterLock.Value.Set();
			}
		}
	}
}
