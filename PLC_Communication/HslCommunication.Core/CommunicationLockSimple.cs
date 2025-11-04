using System;
using System.Threading;

namespace HslCommunication.Core;

public class CommunicationLockSimple : CommunicationLockNone
{
	private int m_waiters = 0;

	private readonly AutoResetEvent m_waiterLock = new AutoResetEvent(initialState: false);

	public bool IsWaitting => m_waiters != 0;

	public override OperateResult EnterLock(int timeout)
	{
		try
		{
			if (Interlocked.Increment(ref m_waiters) == 1)
			{
				return OperateResult.CreateSuccessResult();
			}
			return m_waiterLock.WaitOne(timeout) ? OperateResult.CreateSuccessResult() : new OperateResult($"Enter lock failed, timeout: {timeout}");
		}
		catch (Exception ex)
		{
			return new OperateResult("Enter lock failed, message: " + ex.Message);
		}
	}

	public override void LeaveLock()
	{
		if (Interlocked.Decrement(ref m_waiters) != 0)
		{
			bool flag = m_waiterLock.Set();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
		}
		m_waiterLock.Close();
	}

	public override string ToString()
	{
		return "CommunicationLockSimple[" + (IsWaitting ? "Locking" : "Unlock") + "]";
	}
}
