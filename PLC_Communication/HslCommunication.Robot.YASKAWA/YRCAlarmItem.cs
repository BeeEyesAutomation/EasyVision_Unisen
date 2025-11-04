using System;
using System.Text;
using HslCommunication.Core;

namespace HslCommunication.Robot.YASKAWA;

public class YRCAlarmItem
{
	public int AlarmCode { get; set; }

	public DateTime Time { get; set; }

	public string Message { get; set; }

	public YRCAlarmItem()
	{
	}

	public YRCAlarmItem(IByteTransform byteTransform, byte[] content, Encoding encoding)
	{
		AlarmCode = byteTransform.TransInt32(content, 0);
		Time = Convert.ToDateTime(Encoding.ASCII.GetString(content, 16, 16));
		Message = encoding.GetString(content.RemoveBegin(32));
	}

	public override string ToString()
	{
		return $"[{AlarmCode}] Time:[{Time}] {Message}";
	}
}
