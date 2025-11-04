using System;
using System.Threading;

namespace HslCommunication.Core;

internal sealed class AsyncCoordinator
{
	private int m_opCount = 1;

	private int m_statusReported = 0;

	private Action<CoordinationStatus> m_callback;

	private Timer m_timer;

	public void AboutToBegin(int opsToAdd = 1)
	{
		Interlocked.Add(ref m_opCount, opsToAdd);
	}

	public void JustEnded()
	{
		if (Interlocked.Decrement(ref m_opCount) == 0)
		{
			ReportStatus(CoordinationStatus.AllDone);
		}
	}

	public void AllBegun(Action<CoordinationStatus> callback, int timeout = -1)
	{
		m_callback = callback;
		if (timeout != -1)
		{
			m_timer = new Timer(TimeExpired, null, timeout, -1);
		}
		JustEnded();
	}

	private void TimeExpired(object o)
	{
		ReportStatus(CoordinationStatus.Timeout);
	}

	public void Cancel()
	{
		ReportStatus(CoordinationStatus.Cancel);
	}

	private void ReportStatus(CoordinationStatus status)
	{
		if (Interlocked.Exchange(ref m_statusReported, 1) == 0)
		{
			m_callback(status);
		}
	}

	public static int Maxinum(ref int target, Func<int, int> change)
	{
		int num = target;
		int num2;
		int num3;
		do
		{
			num2 = num;
			num3 = change(num2);
			num = Interlocked.CompareExchange(ref target, num3, num2);
		}
		while (num2 != num);
		return num3;
	}
}
