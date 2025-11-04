using System;

namespace HslCommunication.MQTT;

public class MqttClientApplicationMessage : MqttApplicationMessage
{
	public string ClientId { get; set; }

	public string UserName { get; set; }

	public bool IsCancelPublish { get; set; } = false;

	public DateTime CreateTime { get; set; }

	public int MsgID { get; set; }

	public MqttClientApplicationMessage()
	{
		CreateTime = DateTime.Now;
	}
}
