namespace HslCommunication.MQTT;

public class MqttPublishMessage
{
	public bool IsSendFirstTime { get; set; }

	public int Identifier { get; set; }

	public MqttApplicationMessage Message { get; set; }

	public MqttPublishMessage()
	{
		IsSendFirstTime = true;
	}
}
