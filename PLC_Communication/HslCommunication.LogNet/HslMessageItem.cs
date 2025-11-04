using System;
using System.Threading;

namespace HslCommunication.LogNet;

public class HslMessageItem
{
	private static long IdNumber;

	internal int WriteRetryTimes = 1;

	internal bool HasLogOutput = false;

	public long Id { get; private set; }

	public HslMessageDegree Degree { get; set; } = HslMessageDegree.DEBUG;

	public int ThreadId { get; set; }

	public string Text { get; set; }

	public DateTime Time { get; set; }

	public string KeyWord { get; set; }

	public bool Cancel { get; set; }

	public HslMessageItem()
	{
		Id = Interlocked.Increment(ref IdNumber);
		Time = DateTime.Now;
	}

	public override string ToString()
	{
		if (Degree != HslMessageDegree.None)
		{
			if (string.IsNullOrEmpty(KeyWord))
			{
				return $"[{LogNetManagment.GetDegreeDescription(Degree)}] {Time:yyyy-MM-dd HH:mm:ss.fff} Thread [{ThreadId:D3}] {Text}";
			}
			return $"[{LogNetManagment.GetDegreeDescription(Degree)}] {Time:yyyy-MM-dd HH:mm:ss.fff} Thread [{ThreadId:D3}] {KeyWord} : {Text}";
		}
		return Text;
	}

	public string ToStringWithoutKeyword()
	{
		if (Degree != HslMessageDegree.None)
		{
			return $"[{LogNetManagment.GetDegreeDescription(Degree)}] {Time:yyyy-MM-dd HH:mm:ss.fff} Thread [{ThreadId:D3}] {Text}";
		}
		return Text;
	}
}
