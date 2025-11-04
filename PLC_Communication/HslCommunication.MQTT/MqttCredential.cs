namespace HslCommunication.MQTT;

public class MqttCredential
{
	public string UserName { get; set; }

	public string Password { get; set; }

	public MqttCredential()
	{
	}

	public MqttCredential(string name, string pwd)
	{
		UserName = name;
		Password = pwd;
	}

	public override string ToString()
	{
		return UserName;
	}
}
