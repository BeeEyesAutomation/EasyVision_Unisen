using System;

namespace HslCommunication.Core;

public class CommunicationLockNone : ICommunicationLock, IDisposable
{
	private bool disposedValue = false;

	public virtual OperateResult EnterLock(int timeout)
	{
		return OperateResult.CreateSuccessResult();
	}

	public virtual void LeaveLock()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
		}
	}

	public void Dispose()
	{
		if (!disposedValue)
		{
			Dispose(disposing: true);
			disposedValue = true;
		}
	}
}
