using System;

namespace HslCommunication.Core.Pipe;

public class PipeBase : IDisposable
{
	private SimpleHybirdLock hybirdLock;

	public int LockingTick => hybirdLock.LockingTick;

	public PipeBase()
	{
		hybirdLock = new SimpleHybirdLock();
	}

	public bool PipeLockEnter()
	{
		return hybirdLock.Enter();
	}

	public bool PipeLockLeave()
	{
		return hybirdLock.Leave();
	}

	public virtual void Dispose()
	{
		hybirdLock?.Dispose();
	}
}
