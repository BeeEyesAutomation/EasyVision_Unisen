using System;

namespace HslCommunication.Reflection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class HslRedisHashFieldAttribute : Attribute
{
	public string HaskKey { get; set; }

	public string Field { get; set; }

	public HslRedisHashFieldAttribute(string hashKey, string filed)
	{
		HaskKey = hashKey;
		Field = filed;
	}
}
