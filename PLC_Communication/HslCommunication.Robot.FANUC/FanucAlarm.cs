using System;
using System.Text;
using HslCommunication.Core;

namespace HslCommunication.Robot.FANUC;

public class FanucAlarm
{
	public short AlarmID { get; set; }

	public short AlarmNumber { get; set; }

	public short CauseAlarmID { get; set; }

	public short CauseAlarmNumber { get; set; }

	public short Severity { get; set; }

	public DateTime Time { get; set; }

	public string AlarmMessage { get; set; }

	public string CauseAlarmMessage { get; set; }

	public string SeverityMessage { get; set; }

	public void LoadByContent(IByteTransform byteTransform, byte[] content, int index, Encoding encoding)
	{
		AlarmID = BitConverter.ToInt16(content, index);
		AlarmNumber = BitConverter.ToInt16(content, index + 2);
		CauseAlarmID = BitConverter.ToInt16(content, index + 4);
		CauseAlarmNumber = BitConverter.ToInt16(content, index + 6);
		Severity = BitConverter.ToInt16(content, index + 8);
		if (BitConverter.ToInt16(content, index + 10) > 0)
		{
			Time = new DateTime(BitConverter.ToInt16(content, index + 10), BitConverter.ToInt16(content, index + 12), BitConverter.ToInt16(content, index + 14), BitConverter.ToInt16(content, index + 16), BitConverter.ToInt16(content, index + 18), BitConverter.ToInt16(content, index + 20));
		}
		AlarmMessage = encoding.GetString(content, index + 22, 80).Trim(default(char));
		CauseAlarmMessage = encoding.GetString(content, index + 102, 80).Trim(default(char));
		SeverityMessage = encoding.GetString(content, index + 182, 18).Trim(default(char));
	}

	public override string ToString()
	{
		return $"FanucAlarm ID[{AlarmID},{AlarmNumber},{CauseAlarmID},{CauseAlarmNumber},{Severity}]{Environment.NewLine}{AlarmMessage}{Environment.NewLine}{CauseAlarmMessage}{Environment.NewLine}{SeverityMessage}";
	}

	public static FanucAlarm PraseFrom(IByteTransform byteTransform, byte[] content, int index, Encoding encoding)
	{
		FanucAlarm fanucAlarm = new FanucAlarm();
		fanucAlarm.LoadByContent(byteTransform, content, index, encoding);
		return fanucAlarm;
	}
}
