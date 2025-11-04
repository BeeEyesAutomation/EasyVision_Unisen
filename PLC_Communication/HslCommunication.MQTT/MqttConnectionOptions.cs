using System;
using HslCommunication.Core;

namespace HslCommunication.MQTT;

public class MqttConnectionOptions
{
	private string ipAddress = "127.0.0.1";

	private string hostName = string.Empty;

	public string IpAddress
	{
		get
		{
			return ipAddress;
		}
		set
		{
			hostName = value;
			ipAddress = HslHelper.GetIpAddressFromInput(value);
		}
	}

	public int Port { get; set; }

	public string ClientId { get; set; }

	public int ConnectTimeout { get; set; }

	public MqttApplicationMessage WillMessage { get; set; }

	public MqttCredential Credentials { get; set; }

	public string CertificateFile { get; set; }

	public bool UseSSL { get; set; }

	public bool SSLSecure { get; set; }

	public TimeSpan KeepAlivePeriod { get; set; }

	public TimeSpan KeepAliveSendInterval { get; set; }

	public bool CleanSession { get; set; }

	public bool UseRSAProvider { get; set; }

	public string HostName => hostName;

	public MqttConnectionOptions()
	{
		ClientId = string.Empty;
		IpAddress = "127.0.0.1";
		Port = 1883;
		KeepAlivePeriod = TimeSpan.FromSeconds(100.0);
		KeepAliveSendInterval = TimeSpan.FromSeconds(30.0);
		CleanSession = true;
		ConnectTimeout = 5000;
	}
}
