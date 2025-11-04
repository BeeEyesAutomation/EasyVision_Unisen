using System;
using System.Net;
using System.Threading;

namespace HslCommunication.Core;

public class MqttFileMonitorItem
{
	private static long uniqueIdCreate;

	public long UniqueId { get; private set; }

	public IPEndPoint EndPoint { get; set; }

	public string ClientId { get; set; }

	public string UserName { get; set; }

	public string Operate { get; set; }

	public string Groups { get; set; }

	public string FileName { get; set; }

	public string MappingName { get; set; }

	public long SpeedSecond { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime LastUpdateTime { get; set; }

	public long TotalSize { get; set; }

	public long LastUpdateProgress { get; set; }

	public MqttFileMonitorItem()
	{
		StartTime = DateTime.Now;
		LastUpdateTime = DateTime.Now;
		UniqueId = Interlocked.Increment(ref uniqueIdCreate);
	}

	public void UpdateProgress(long progress, long total)
	{
		TotalSize = total;
		TimeSpan timeSpan = DateTime.Now - LastUpdateTime;
		if (timeSpan.TotalSeconds >= 0.2)
		{
			long num = progress - LastUpdateProgress;
			if (num <= 0)
			{
				SpeedSecond = 0L;
				return;
			}
			SpeedSecond = (long)((double)num / timeSpan.TotalSeconds);
			LastUpdateTime = DateTime.Now;
			LastUpdateProgress = progress;
		}
	}
}
