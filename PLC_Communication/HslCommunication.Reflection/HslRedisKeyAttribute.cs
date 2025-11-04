using System;

namespace HslCommunication.Reflection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class HslRedisKeyAttribute : Attribute
{
	public string KeyName { get; set; }

	public HslRedisKeyAttribute(string key)
	{
		KeyName = key;
	}
}
