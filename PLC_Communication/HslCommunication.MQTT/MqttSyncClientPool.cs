using System;
using System.Threading.Tasks;
using HslCommunication.Algorithms.ConnectPool;

namespace HslCommunication.MQTT;

public class MqttSyncClientPool
{
	private MqttConnectionOptions connectionOptions;

	private ConnectPool<IMqttSyncConnector> mqttConnectPool;

	public ConnectPool<IMqttSyncConnector> GetMqttSyncConnectPool => mqttConnectPool;

	public int MaxConnector
	{
		get
		{
			return mqttConnectPool.MaxConnector;
		}
		set
		{
			mqttConnectPool.MaxConnector = value;
		}
	}

	public MqttSyncClientPool(MqttConnectionOptions options)
	{
		connectionOptions = options;
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			mqttConnectPool = new ConnectPool<IMqttSyncConnector>(() => new IMqttSyncConnector(options));
			mqttConnectPool.MaxConnector = int.MaxValue;
			return;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	public MqttSyncClientPool(MqttConnectionOptions options, Action<MqttSyncClient> initialize)
	{
		connectionOptions = options;
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			mqttConnectPool = new ConnectPool<IMqttSyncConnector>(delegate
			{
				MqttSyncClient mqttSyncClient = new MqttSyncClient(options);
				initialize(mqttSyncClient);
				return new IMqttSyncConnector
				{
					SyncClient = mqttSyncClient
				};
			});
			mqttConnectPool.MaxConnector = int.MaxValue;
			return;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	private OperateResult<T> ConnectPoolExecute<T>(Func<MqttSyncClient, OperateResult<T>> exec)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			IMqttSyncConnector availableConnector = mqttConnectPool.GetAvailableConnector();
			OperateResult<T> result = exec(availableConnector.SyncClient);
			mqttConnectPool.ReturnConnector(availableConnector);
			return result;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	private OperateResult<T1, T2> ConnectPoolExecute<T1, T2>(Func<MqttSyncClient, OperateResult<T1, T2>> exec)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			IMqttSyncConnector availableConnector = mqttConnectPool.GetAvailableConnector();
			OperateResult<T1, T2> result = exec(availableConnector.SyncClient);
			mqttConnectPool.ReturnConnector(availableConnector);
			return result;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	private async Task<OperateResult<T>> ConnectPoolExecuteAsync<T>(Func<MqttSyncClient, Task<OperateResult<T>>> exec)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			IMqttSyncConnector client = mqttConnectPool.GetAvailableConnector();
			OperateResult<T> result = await exec(client.SyncClient);
			mqttConnectPool.ReturnConnector(client);
			return result;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	private async Task<OperateResult<T1, T2>> ConnectPoolExecuteAsync<T1, T2>(Func<MqttSyncClient, Task<OperateResult<T1, T2>>> execAsync)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			IMqttSyncConnector client = mqttConnectPool.GetAvailableConnector();
			OperateResult<T1, T2> result = await execAsync(client.SyncClient);
			mqttConnectPool.ReturnConnector(client);
			return result;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	public OperateResult<string, byte[]> Read(string topic, byte[] payload, Action<long, long> sendProgress = null, Action<string, string> handleProgress = null, Action<long, long> receiveProgress = null)
	{
		return ConnectPoolExecute((MqttSyncClient m) => m.Read(topic, payload, sendProgress, handleProgress, receiveProgress));
	}

	public OperateResult<string, string> ReadString(string topic, string payload, Action<long, long> sendProgress = null, Action<string, string> handleProgress = null, Action<long, long> receiveProgress = null)
	{
		return ConnectPoolExecute((MqttSyncClient m) => m.ReadString(topic, payload, sendProgress, handleProgress, receiveProgress));
	}

	public OperateResult<T> ReadRpc<T>(string topic, string payload)
	{
		return ConnectPoolExecute((MqttSyncClient m) => m.ReadRpc<T>(topic, payload));
	}

	public OperateResult<T> ReadRpc<T>(string topic, object payload)
	{
		return ConnectPoolExecute((MqttSyncClient m) => m.ReadRpc<T>(topic, payload));
	}

	public OperateResult<MqttRpcApiInfo[]> ReadRpcApis()
	{
		return ConnectPoolExecute((MqttSyncClient m) => m.ReadRpcApis());
	}

	public OperateResult<long[]> ReadRpcApiLog(string api)
	{
		return ConnectPoolExecute((MqttSyncClient m) => m.ReadRpcApiLog(api));
	}

	public OperateResult<string[]> ReadRetainTopics()
	{
		return ConnectPoolExecute((MqttSyncClient m) => m.ReadRetainTopics());
	}

	public OperateResult<MqttClientApplicationMessage> ReadTopicPayload(string topic, Action<long, long> receiveProgress = null)
	{
		return ConnectPoolExecute((MqttSyncClient m) => m.ReadTopicPayload(topic, receiveProgress));
	}

	public async Task<OperateResult<string, byte[]>> ReadAsync(string topic, byte[] payload, Action<long, long> sendProgress = null, Action<string, string> handleProgress = null, Action<long, long> receiveProgress = null)
	{
		return await ConnectPoolExecuteAsync((MqttSyncClient m) => m.ReadAsync(topic, payload, sendProgress, handleProgress, receiveProgress));
	}

	public async Task<OperateResult<string, string>> ReadStringAsync(string topic, string payload, Action<long, long> sendProgress = null, Action<string, string> handleProgress = null, Action<long, long> receiveProgress = null)
	{
		return await ConnectPoolExecuteAsync((MqttSyncClient m) => m.ReadStringAsync(topic, payload, sendProgress, handleProgress, receiveProgress));
	}

	public async Task<OperateResult<T>> ReadRpcAsync<T>(string topic, string payload)
	{
		return await ConnectPoolExecuteAsync((MqttSyncClient m) => m.ReadRpcAsync<T>(topic, payload));
	}

	public async Task<OperateResult<T>> ReadRpcAsync<T>(string topic, object payload)
	{
		return await ConnectPoolExecuteAsync((MqttSyncClient m) => m.ReadRpcAsync<T>(topic, payload));
	}

	public async Task<OperateResult<MqttRpcApiInfo[]>> ReadRpcApisAsync()
	{
		return await ConnectPoolExecuteAsync((MqttSyncClient m) => m.ReadRpcApisAsync());
	}

	public async Task<OperateResult<long[]>> ReadRpcApiLogAsync(string api)
	{
		return await ConnectPoolExecuteAsync((MqttSyncClient m) => m.ReadRpcApiLogAsync(api));
	}

	public async Task<OperateResult<string[]>> ReadRetainTopicsAsync()
	{
		return await ConnectPoolExecuteAsync((MqttSyncClient m) => m.ReadRetainTopicsAsync());
	}

	public async Task<OperateResult<MqttClientApplicationMessage>> ReadTopicPayloadAsync(string topic, Action<long, long> receiveProgress = null)
	{
		return await ConnectPoolExecuteAsync((MqttSyncClient m) => m.ReadTopicPayloadAsync(topic, receiveProgress));
	}

	public override string ToString()
	{
		return $"MqttSyncClientPool[{mqttConnectPool.MaxConnector}]";
	}
}
