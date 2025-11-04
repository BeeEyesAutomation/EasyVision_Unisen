using HslCommunication.BasicFramework;

namespace HslCommunication.MQTT;

public class MqttSubscribeMessage
{
	public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

	public int Identifier { get; set; }

	public string[] Topics { get; set; }

	public MqttSubscribeMessage()
	{
		QualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce;
		Identifier = 1;
	}

	public override string ToString()
	{
		return "MqttSubcribeMessage" + SoftBasic.ArrayFormat(Topics);
	}
}
