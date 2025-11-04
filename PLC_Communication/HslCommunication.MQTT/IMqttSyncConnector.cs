using System;
using HslCommunication.Algorithms.ConnectPool;

namespace HslCommunication.MQTT;

public class IMqttSyncConnector : IConnector
{
	private MqttConnectionOptions connectionOptions;

	public bool IsConnectUsing { get; set; }

	public string GuidToken { get; set; }

	public DateTime LastUseTime { get; set; }

	public MqttSyncClient SyncClient { get; set; }

	public IMqttSyncConnector(MqttConnectionOptions options)
	{
		connectionOptions = options;
		SyncClient = new MqttSyncClient(options);
	}

	public IMqttSyncConnector()
	{
	}

	public void Close()
	{
		SyncClient?.ConnectClose();
	}

	public void Open()
	{
	}
}
