using System;

namespace HslCommunication.Reflection;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
public class HslMqttApiAttribute : Attribute
{
	public string ApiTopic { get; set; }

	public string Description { get; set; }

	public bool PropertyUnfold { get; set; } = false;

	public string HttpMethod { get; set; } = "POST";

	public HslMqttApiAttribute(string description)
	{
		Description = description;
	}

	public HslMqttApiAttribute(string apiTopic, string description)
	{
		ApiTopic = apiTopic;
		Description = description;
	}

	public HslMqttApiAttribute()
	{
	}
}
