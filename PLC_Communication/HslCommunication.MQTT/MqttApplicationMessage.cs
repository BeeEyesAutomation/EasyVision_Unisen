using System.Text;

namespace HslCommunication.MQTT;

public class MqttApplicationMessage
{
	public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

	public string Topic { get; set; }

	public byte[] Payload { get; set; }

	public bool Retain { get; set; }

	public override string ToString()
	{
		return Topic + ":" + Encoding.UTF8.GetString(Payload);
	}
}
