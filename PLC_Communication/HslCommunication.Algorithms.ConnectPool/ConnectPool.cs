using System;
using System.Collections.Generic;
using System.Threading;
using HslCommunication.Core;

namespace HslCommunication.Algorithms.ConnectPool;

public class ConnectPool<TConnector> where TConnector : IConnector
{
	private Func<TConnector> CreateConnector = null;

	private int maxConnector = 10;

	private int usedConnector = 0;

	private int usedConnectorMax = 0;

	private int expireTime = 30;

	private bool canGetConnector = true;

	private Timer timerCheck = null;

	private object listLock;

	private List<TConnector> connectors = null;

	public int MaxConnector
	{
		get
		{
			return maxConnector;
		}
		set
		{
			maxConnector = value;
		}
	}

	public int ConectionExpireTime
	{
		get
		{
			return expireTime;
		}
		set
		{
			expireTime = value;
		}
	}

	public int UsedConnector => usedConnector;

	public int UseConnectorMax => usedConnectorMax;

	public ConnectPool(Func<TConnector> createConnector)
	{
		CreateConnector = createConnector;
		listLock = new object();
		connectors = new List<TConnector>();
		timerCheck = new Timer(TimerCheckBackground, null, 10000, 30000);
	}

	public TConnector GetAvailableConnector()
	{
		while (!canGetConnector)
		{
			HslHelper.ThreadSleep(20);
		}
		TConnector val = default(TConnector);
		lock (listLock)
		{
			for (int i = 0; i < connectors.Count; i++)
			{
				if (!connectors[i].IsConnectUsing)
				{
					TConnector val2 = connectors[i];
					val2.IsConnectUsing = true;
					val = connectors[i];
					break;
				}
			}
			TConnector val4;
			if (val == null)
			{
				val = CreateConnector();
				val.IsConnectUsing = true;
				TConnector val3 = val;
				if (default(TConnector) == null)
				{
					TConnector val2 = val3;
					val3 = val2;
				}
				ref TConnector reference = ref val3;
				val4 = default(TConnector);
				if (val4 == null)
				{
					val4 = reference;
					reference = ref val4;
				}
				DateTime now = DateTime.Now;
				reference.LastUseTime = now;
				val.Open();
				connectors.Add(val);
				usedConnector = connectors.Count;
				if (usedConnector > usedConnectorMax)
				{
					usedConnectorMax = usedConnector;
				}
				if (usedConnector == maxConnector)
				{
					canGetConnector = false;
				}
			}
			TConnector val5 = val;
			if (default(TConnector) == null)
			{
				TConnector val2 = val5;
				val5 = val2;
			}
			ref TConnector reference2 = ref val5;
			val4 = default(TConnector);
			if (val4 == null)
			{
				val4 = reference2;
				reference2 = ref val4;
			}
			DateTime now2 = DateTime.Now;
			reference2.LastUseTime = now2;
		}
		return val;
	}

	public void ReturnConnector(TConnector connector)
	{
		lock (listLock)
		{
			int num = connectors.IndexOf(connector);
			if (num != -1)
			{
				TConnector val = connectors[num];
				val.IsConnectUsing = false;
			}
		}
	}

	public void ResetAllConnector()
	{
		lock (listLock)
		{
			for (int num = connectors.Count - 1; num >= 0; num--)
			{
				connectors[num].Close();
				connectors.RemoveAt(num);
			}
		}
	}

	private void TimerCheckBackground(object obj)
	{
		lock (listLock)
		{
			for (int num = connectors.Count - 1; num >= 0; num--)
			{
				if ((DateTime.Now - connectors[num].LastUseTime).TotalSeconds > (double)expireTime && !connectors[num].IsConnectUsing)
				{
					connectors[num].Close();
					connectors.RemoveAt(num);
				}
			}
			usedConnector = connectors.Count;
			if (usedConnector < MaxConnector)
			{
				canGetConnector = true;
			}
		}
	}
}
