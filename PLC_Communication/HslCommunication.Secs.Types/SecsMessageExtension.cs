using System;
using System.Text;

namespace HslCommunication.Secs.Types;

public static class SecsMessageExtension
{
	public static string ToRenderString(this SecsValue[] secsMessages)
	{
		if (secsMessages == null || secsMessages.Length == 0)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (SecsValue secsValue in secsMessages)
		{
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(secsValue.ToString());
		}
		return stringBuilder.ToString();
	}
}
