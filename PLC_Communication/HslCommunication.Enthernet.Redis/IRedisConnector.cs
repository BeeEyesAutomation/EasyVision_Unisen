using System;
using HslCommunication.Algorithms.ConnectPool;

namespace HslCommunication.Enthernet.Redis;

public class IRedisConnector : IConnector
{
	public bool IsConnectUsing { get; set; }

	public string GuidToken { get; set; }

	public DateTime LastUseTime { get; set; }

	public RedisClient Redis { get; set; }

	public void Close()
	{
		Redis?.ConnectClose();
	}

	public void Open()
	{
		Redis?.SetPersistentConnection();
	}
}
