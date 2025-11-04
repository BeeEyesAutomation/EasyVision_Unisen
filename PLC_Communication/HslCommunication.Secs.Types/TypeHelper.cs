using System;

namespace HslCommunication.Secs.Types;

internal class TypeHelper
{
	public static void TypeListCheck(SecsValue secsItem)
	{
		if (secsItem.ItemType != SecsItemType.List)
		{
			throw new InvalidCastException($"Current type must be List, but now is {secsItem.ItemType} {Environment.NewLine} Source: {secsItem.ToXElement()}");
		}
	}
}
