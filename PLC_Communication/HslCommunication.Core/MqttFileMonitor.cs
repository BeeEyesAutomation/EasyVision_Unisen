using System.Collections.Generic;
using System.Linq;

namespace HslCommunication.Core;

public class MqttFileMonitor
{
	private Dictionary<long, MqttFileMonitorItem> fileMonitors;

	private object dicLock;

	public MqttFileMonitor()
	{
		dicLock = new object();
		fileMonitors = new Dictionary<long, MqttFileMonitorItem>();
	}

	public void Add(MqttFileMonitorItem monitorItem)
	{
		lock (dicLock)
		{
			if (fileMonitors.ContainsKey(monitorItem.UniqueId))
			{
				fileMonitors[monitorItem.UniqueId] = monitorItem;
			}
			else
			{
				fileMonitors.Add(monitorItem.UniqueId, monitorItem);
			}
		}
	}

	public void Remove(long uniqueId)
	{
		lock (dicLock)
		{
			if (fileMonitors.ContainsKey(uniqueId))
			{
				fileMonitors.Remove(uniqueId);
			}
		}
	}

	public MqttFileMonitorItem[] GetMonitorItemsSnapShoot()
	{
		lock (dicLock)
		{
			return fileMonitors.Values.ToArray();
		}
	}
}
