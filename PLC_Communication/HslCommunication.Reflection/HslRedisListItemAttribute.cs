using System;

namespace HslCommunication.Reflection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class HslRedisListItemAttribute : Attribute
{
	public string ListKey { get; set; }

	public long Index { get; set; }

	public HslRedisListItemAttribute(string listKey, long index)
	{
		ListKey = listKey;
		Index = index;
	}
}
