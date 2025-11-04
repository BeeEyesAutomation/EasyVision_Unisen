using System;
using System.Text;

namespace HslCommunication.MQTT;

public class MqttSessionInfo
{
	public string EndPoint { get; set; }

	public string ClientId { get; set; }

	public DateTime ActiveTime { get; set; }

	public DateTime OnlineTime { get; set; }

	public string[] Topics { get; set; }

	public string UserName { get; set; }

	public string Protocol { get; set; }

	public string WillTopic { get; set; }

	public bool DeveloperPermissions { get; set; }

	public bool IsAesCryptography { get; set; }

	public bool ForbidPublishTopic { get; set; }

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(Protocol + " Session[IP:" + EndPoint + "]");
		if (!string.IsNullOrEmpty(ClientId))
		{
			stringBuilder.Append(" [ID:" + ClientId + "]");
		}
		if (!string.IsNullOrEmpty(UserName))
		{
			stringBuilder.Append(" [Name:" + UserName + "]");
		}
		if (IsAesCryptography)
		{
			stringBuilder.Append("[RSA/AES]");
		}
		return stringBuilder.ToString();
	}
}
