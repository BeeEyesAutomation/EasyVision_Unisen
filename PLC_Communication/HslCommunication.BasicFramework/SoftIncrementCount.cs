using System;
using HslCommunication.Core;

namespace HslCommunication.BasicFramework;

public sealed class SoftIncrementCount : IDisposable
{
	private long start = 0L;

	private long current = 0L;

	private long max = long.MaxValue;

	private SimpleHybirdLock hybirdLock;

	private bool disposedValue = false;

	public int IncreaseTick { get; set; } = 1;

	public long MaxValue => max;

	public SoftIncrementCount(long max, long start = 0L, int tick = 1)
	{
		this.start = start;
		this.max = max;
		current = start;
		IncreaseTick = tick;
		hybirdLock = new SimpleHybirdLock();
	}

	public long GetCurrentValue()
	{
		long num = 0L;
		hybirdLock.Enter();
		num = current;
		current += IncreaseTick;
		if (current > max)
		{
			current = start;
		}
		else if (current < start)
		{
			current = max;
		}
		hybirdLock.Leave();
		return num;
	}

	public void ResetMaxValue(long max)
	{
		hybirdLock.Enter();
		if (max > start)
		{
			if (max < current)
			{
				current = start;
			}
			this.max = max;
		}
		hybirdLock.Leave();
	}

	public void ResetStartValue(long start)
	{
		hybirdLock.Enter();
		if (start < max)
		{
			if (current < start)
			{
				current = start;
			}
			this.start = start;
		}
		hybirdLock.Leave();
	}

	public void ResetCurrentValue()
	{
		hybirdLock.Enter();
		current = start;
		hybirdLock.Leave();
	}

	public void ResetCurrentValue(long value)
	{
		hybirdLock.Enter();
		if (value > max)
		{
			current = max;
		}
		else if (value < start)
		{
			current = start;
		}
		else
		{
			current = value;
		}
		hybirdLock.Leave();
	}

	public override string ToString()
	{
		return $"SoftIncrementCount[{current}]";
	}

	private void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				hybirdLock.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}
}
