using System;

namespace HslCommunication.Reflection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class HslRedisListAttribute : Attribute
{
	public string ListKey { get; set; }

	public long StartIndex { get; set; }

	public long EndIndex { get; set; } = -1L;

	public HslRedisListAttribute(string listKey)
	{
		ListKey = listKey;
	}

	public HslRedisListAttribute(string listKey, long startIndex)
	{
		ListKey = listKey;
		StartIndex = startIndex;
	}

	public HslRedisListAttribute(string listKey, long startIndex, long endIndex)
	{
		ListKey = listKey;
		StartIndex = startIndex;
		EndIndex = endIndex;
	}
}
