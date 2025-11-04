using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using HslCommunication.Core;
using HslCommunication.LogNet;
using HslCommunication.Reflection;
using Newtonsoft.Json;

namespace HslCommunication;

public class HslTimeOut
{
	private static long hslTimeoutId;

	private static List<HslTimeOut> WaitHandleTimeOut;

	private static object listLock;

	private static Thread threadCheckTimeOut;

	private static long threadUniqueId;

	private static DateTime threadActiveTime;

	private static int activeDisableCount;

	public static long TimeoutDealCount;

	private static bool isLinuxPlatform;

	public long UniqueId { get; private set; }

	public DateTime StartTime { get; set; }

	public bool IsSuccessful { get; set; }

	public int DelayTime { get; set; }

	[JsonIgnore]
	public Socket WorkSocket { get; set; }

	public bool IsTimeout { get; set; }

	public static ILogNet TimeoutLogNet { get; set; }

	[HslMqttApi(Description = "Get the number of current check timeout objects", HttpMethod = "GET")]
	public static int TimeOutCheckCount => WaitHandleTimeOut.Count;

	public HslTimeOut()
	{
		UniqueId = Interlocked.Increment(ref hslTimeoutId);
		StartTime = DateTime.Now;
		IsSuccessful = false;
		IsTimeout = false;
	}

	public TimeSpan GetConsumeTime()
	{
		return DateTime.Now - StartTime;
	}

	public override string ToString()
	{
		return $"HslTimeOut[{DelayTime}]";
	}

	public static void HandleTimeOutCheck(HslTimeOut timeOut)
	{
		lock (listLock)
		{
			if ((DateTime.Now - threadActiveTime).TotalSeconds > 60.0)
			{
				threadActiveTime = DateTime.Now;
				if (Interlocked.Increment(ref activeDisableCount) >= 2)
				{
					CreateTimeoutCheckThread();
				}
			}
			WaitHandleTimeOut.Add(timeOut);
		}
	}

	[HslMqttApi(Description = "Get the current list of all waiting timeout check objects, do not manually change the property value of the object", HttpMethod = "GET")]
	public static HslTimeOut[] GetHslTimeOutsSnapShoot()
	{
		lock (listLock)
		{
			return WaitHandleTimeOut.ToArray();
		}
	}

	public static HslTimeOut HandleTimeOutCheck(Socket socket, int timeout)
	{
		HslTimeOut hslTimeOut = new HslTimeOut
		{
			DelayTime = timeout,
			IsSuccessful = false,
			StartTime = DateTime.Now,
			WorkSocket = socket
		};
		if (timeout > 0)
		{
			HandleTimeOutCheck(hslTimeOut);
		}
		return hslTimeOut;
	}

	static HslTimeOut()
	{
		hslTimeoutId = 0L;
		WaitHandleTimeOut = new List<HslTimeOut>(128);
		listLock = new object();
		threadUniqueId = 0L;
		activeDisableCount = 0;
		TimeoutDealCount = 0L;
		isLinuxPlatform = false;
		CreateTimeoutCheckThread();
	}

	private static void CreateTimeoutCheckThread()
	{
		threadActiveTime = DateTime.Now;
		try
		{
			threadCheckTimeOut?.Abort();
		}
		catch
		{
		}
		threadCheckTimeOut = new Thread(CheckTimeOut);
		threadCheckTimeOut.IsBackground = true;
		threadCheckTimeOut.Priority = ThreadPriority.AboveNormal;
		threadCheckTimeOut.Start(Interlocked.Increment(ref threadUniqueId));
	}

	private static void CheckTimeOut(object obj)
	{
		long num = (long)obj;
		TimeoutLogNet?.WriteInfo("HslCommunication.TimeoutThread", $"ID[{num}] CheckTimeOut start.");
		while (true)
		{
			HslHelper.ThreadSleep(100);
			if (num != threadUniqueId)
			{
				break;
			}
			threadActiveTime = DateTime.Now;
			activeDisableCount = 0;
			lock (listLock)
			{
				for (int num2 = WaitHandleTimeOut.Count - 1; num2 >= 0; num2--)
				{
					HslTimeOut hslTimeOut = WaitHandleTimeOut[num2];
					if (hslTimeOut.IsSuccessful)
					{
						WaitHandleTimeOut[num2].WorkSocket = null;
						WaitHandleTimeOut.RemoveAt(num2);
						Interlocked.Increment(ref TimeoutDealCount);
					}
					else
					{
						double num3 = (DateTime.Now - hslTimeOut.StartTime).TotalMilliseconds;
						if (num3 < 0.0)
						{
							hslTimeOut.StartTime = DateTime.Now;
							num3 = 0.0;
						}
						if (num3 > (double)hslTimeOut.DelayTime)
						{
							if (!hslTimeOut.IsSuccessful)
							{
								if (isLinuxPlatform)
								{
									try
									{
										hslTimeOut.WorkSocket?.Disconnect(reuseSocket: false);
									}
									catch
									{
									}
								}
								NetSupport.CloseSocket(hslTimeOut.WorkSocket);
								hslTimeOut.IsTimeout = true;
							}
							WaitHandleTimeOut[num2].WorkSocket = null;
							WaitHandleTimeOut.RemoveAt(num2);
							Interlocked.Increment(ref TimeoutDealCount);
						}
					}
				}
			}
		}
		TimeoutLogNet?.WriteWarn("HslCommunication.TimeoutThread", $"ID[{num}] break not same as {threadUniqueId}");
	}
}
