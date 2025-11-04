using System.Threading;

namespace HslCommunication.MQTT;

public class SubscribeTopic
{
	private long subscribeTick = 0L;

	public string Topic { get; set; }

	public long SubscribeTick => subscribeTick;

	public event MqttClient.MqttMessageReceiveDelegate OnMqttMessageReceived;

	public SubscribeTopic(string topic)
	{
		Topic = topic;
	}

	public void TriggerSubscription(MqttClient client, MqttApplicationMessage message)
	{
		this.OnMqttMessageReceived?.Invoke(client, message);
	}

	public long AddSubscribeTick()
	{
		return Interlocked.Increment(ref subscribeTick);
	}

	public long RemoveSubscribeTick()
	{
		return Interlocked.Decrement(ref subscribeTick);
	}

	public override string ToString()
	{
		return "SubscribeTopic[" + Topic + "]";
	}
}
